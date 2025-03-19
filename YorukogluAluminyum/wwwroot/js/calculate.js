$(document).ready(function() {
    // Maden seçimi değiştiğinde yoğunluğu güncelle
    $('#maden').change(function() {
        var selectedDensity = $(this).val();
        $('#density').val(selectedDensity);
    });

    // Ürün tipi seçimi değiştiğinde inputları yönet
    $('#urun').change(function() {
        UrunModul();
    });

    // Hesaplama butonu tıklaması
    $('#calculate').click(function() {
        calculateWeight();
    });

    // UrunModul fonksiyonu (document ready içinde tanımlandı)
    function UrunModul() {
        var urunTipi = $('#urun').val();

        $('#en, #kalinlik, #cap, #ic_cap, #boy').prop('disabled', true).val('');

        if (urunTipi === '1') { // Çubuk
            $('#cap, #boy').prop('disabled', false);
            $('#en, #kalinlik, #ic_cap').val('').prop('disabled', true);
        } else if (urunTipi === '2') { // Lama
            $('#en, #kalinlik, #boy').prop('disabled', false);
            $('#cap, #ic_cap').val('').prop('disabled', true);
        } else if (urunTipi === '3') { // Boru
            $('#cap, #ic_cap, #boy').prop('disabled', false);
            $('#en, #kalinlik').val('').prop('disabled', true);
        }
    }

    // Ağırlık hesaplama fonksiyonu
    function calculateWeight() {
        var urunTipi = $('#urun').val();
        var density = parseFloat($('#density').val());
        var en = parseFloat($('#en').val());
        var kalinlik = parseFloat($('#kalinlik').val());
        var cap = parseFloat($('#cap').val());
        var icCap = parseFloat($('#ic_cap').val());
        var boy = parseFloat($('#boy').val());
        var sonuc = 0;

        if (isNaN(density) || isNaN(boy) || boy <= 0) {
            $('#sonuc').val('Geçersiz değerler');
            return;
        }

        if (urunTipi === '1') { // Çubuk
            if (isNaN(cap) || cap <= 0) {
                $('#sonuc').val('Geçersiz çap');
                return;
            }
            sonuc = (Math.PI * Math.pow(cap / 2, 2) * boy * density) / 1000000;
        } else if (urunTipi === '2') { // Lama
            if (isNaN(en) || isNaN(kalinlik) || en <= 0 || kalinlik <= 0) {
                $('#sonuc').val('Geçersiz en veya kalınlık');
                return;
            }
            sonuc = (en * kalinlik * boy * density) / 1000000;
        } else if (urunTipi === '3') { // Boru
            if (isNaN(cap) || isNaN(icCap) || cap <= 0 || icCap < 0 || icCap >= cap) {
                $('#sonuc').val('Geçersiz çaplar');
                return;
            }

            // Boru için dış çap ve iç çapın karelerini doğru şekilde hesapla
            var outerRadius = cap / 2;
            var innerRadius = icCap / 2;

            // Yuvarlama sadece sonucun çıktığı noktada yapılır
            var outerArea = Math.pow(outerRadius, 2);  // Dış çapın alanı
            var innerArea = Math.pow(innerRadius, 2);  // İç çapın alanı
            sonuc = Math.PI * (outerArea - innerArea) * boy * density / 1000000;
        } else {
            $('#sonuc').val('Ürün tipi seçin');
            return;
        }

        // Sonucu 3 haneli ondalıklı sayıya yuvarlıyoruz
        $('#sonuc').val(sonuc.toFixed(3) + ' kg');
    }


});
