// Generate a random captcha code
function generateCaptcha() {
    const captchaLength = 6;
    const characters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';
    let captcha = '';
    for (let i = 0; i < captchaLength; i++) {
      captcha += characters.charAt(Math.floor(Math.random() * characters.length));
    }
    return captcha;
  }
  
  // Display the captcha in the captcha container
  function displayCaptcha() {
    const captchaContainer = document.getElementById('captcha');
    const captcha = generateCaptcha();
    captchaContainer.textContent = captcha;
  }
  
  // Validate the user-entered captcha
  function validateCaptcha() {
    const captchaValue = document.getElementById('captcha-input').value;
    const captchaContainer = document.getElementById('captcha');
    const captcha = captchaContainer.textContent;
    if (captchaValue.toLowerCase() === captcha.toLowerCase()) {
      return true;
    } else {
      alert('Invalid captcha. Please try again.');
      return false;
    }
  }
  
  document.addEventListener('DOMContentLoaded', function () {
    displayCaptcha();
  
    // Regenerate captcha on click
    document.getElementById('captcha').addEventListener('click', function () {
      displayCaptcha();
    });
  
    // Validate captcha on form submission
    document.getElementById('login-form').addEventListener('submit', function (event) {
      if (!validateCaptcha()) {
        event.preventDefault();
      }
    });
  });

$('#captcha').click(function () {
    generateCaptcha();
});