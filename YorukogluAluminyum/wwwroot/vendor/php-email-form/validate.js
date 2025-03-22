$(document).ready(function () {
  $(".php-email-form").submit(function (event) {
    event.preventDefault(); // Sayfanın yenilenmesini engelle

    // reCAPTCHA doğrulama
    if (!validateRecaptcha()) {
      return false; // reCAPTCHA doğrulama geçmediyse formu gönderme
    }

    $(".loading").removeClass("d-none"); // Yükleniyor animasyonunu göster
    $(".sent-message, .error-message").addClass("d-none"); // Önceki mesajları gizle

    $.ajax({
      url: $(this).attr("action"), // Formun action URL'si
      type: $(this).attr("method"), // Formun method'u (POST olmalı)
      data: $(this).serialize(), // Form verilerini al
      success: function (response) {
        console.log(response);

        // Yükleniyor animasyonunu gizle
        $(".loading").addClass("d-none");

        if (response.success) {
          $(".sent-message").removeClass("d-none").text(response.message); // Başarı mesajını göster
        } else {
          $(".error-message").removeClass("d-none").text("Bir hata oluştu, lütfen tekrar deneyin."); // Hata mesajını göster
        }

        $(".php-email-form")[0].reset(); // Formu temizle

        // reCAPTCHA'yı sıfırlama
        grecaptcha.reset();
      },
      error: function () {
        console.log("Bir hata oluştu.");

        // Yükleniyor animasyonunu gizle
        $(".loading").addClass("d-none");

        $(".error-message").removeClass("d-none").text("Bir hata oluştu, lütfen tekrar deneyin."); // Hata mesajını göster
      }
    });
  });
});

function validateRecaptcha() {
  var response = grecaptcha.getResponse();
  if (!response) {
    alert("Lütfen reCAPTCHA'yı doğrulayın.");
    return false;
  }
  document.getElementById("gRecaptchaResponse").value = response; // reCAPTCHA yanıtını gizli alana ekle
  return true;
}
