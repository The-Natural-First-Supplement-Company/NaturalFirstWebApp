let waitingTime = 60;
let timer;

function sendVerificationCode() {
    // Simulate sending the verification code
    // In a real-world scenario, you would send the code via email or other means
    generateVerificationCode();

    // Disable the button temporarily and start the timer
    const sendVerificationBtn = document.getElementById('send-verification-btn');
    sendVerificationBtn.disabled = true;
    timer = setInterval(updateWaitingTime, 1000);
}

function generateVerificationCode() {
    // Generate a random 6-digit verification code
    const code = Math.floor(100000 + Math.random() * 900000);

    // Display the code in the input field for demonstration purposes
    const verificationCodeInput = document.getElementById('verification-code');
    verificationCodeInput.value = code;
}

function updateWaitingTime() {
    const waitTimeElement = document.getElementById('wait-time');
    if (waitingTime > 0) {
        waitingTime--;
        waitTimeElement.textContent = `Please wait ${waitingTime} seconds for the next verification code.`;
    } else {
        // Enable the button and reset the waiting time
        const sendVerificationBtn = document.getElementById('send-verification-btn');
        sendVerificationBtn.disabled = false;
        waitingTime = 60;
        clearInterval(timer);
        waitTimeElement.textContent = '';
    }
}

function generateInvitationCode() {
    // Generate a random 6-character invitation code
    const characters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';
    let invitationCode = '';
    for (let i = 0; i < 6; i++) {
        invitationCode += characters.charAt(Math.floor(Math.random() * characters.length));
    }

    // Display the code in the input field for demonstration purposes
    const invitationCodeInput = document.getElementById('invitation-code');
    invitationCodeInput.value = invitationCode;
}