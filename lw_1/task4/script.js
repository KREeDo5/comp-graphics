let words = [];
let currentWord = "";
let currentQuestion = "";
let guessedLetters = [];
let mistakes = 0;
const maxMistakes = 7;

const canvas = document.getElementById("hangmanCanvas");
const ctx = canvas.getContext("2d");
const wordDisplay = document.getElementById("wordDisplay");
const message = document.getElementById("message");
const restartBtn = document.getElementById("restartBtn");
const keyboard = document.getElementById("keyboard");
const questionDiv = document.getElementById("question");
const classicView = document.getElementById("classicView");
const altView = document.getElementById("altView");
const toggleViewBtn = document.getElementById("toggleViewBtn");
const usedLettersDiv = document.getElementById("usedLetters");
const remainingDiv = document.getElementById("remaining");

let isAltView = false;

toggleViewBtn.addEventListener("click", () => {
    isAltView = !isAltView;
    classicView.style.display = isAltView ? "none" : "block";
    altView.style.display = isAltView ? "block" : "none";
    updateDisplay();
    drawHangman(mistakes);
});

function drawHangman(mistakes) {
    ctx.clearRect(0, 0, canvas.width, canvas.height);
    ctx.lineWidth = 2;

    ctx.beginPath();
    ctx.moveTo(10, 240); ctx.lineTo(190, 240);
    ctx.moveTo(40, 240); ctx.lineTo(40, 20);
    ctx.lineTo(120, 20);
    ctx.lineTo(120, 40);
    ctx.stroke();

    if (mistakes > 0) { ctx.beginPath(); ctx.arc(120, 60, 20, 0, Math.PI * 2); ctx.stroke(); }
    if (mistakes > 1) { ctx.beginPath(); ctx.moveTo(120, 80); ctx.lineTo(120, 140); ctx.stroke(); }
    if (mistakes > 2) { ctx.beginPath(); ctx.moveTo(120, 140); ctx.lineTo(100, 180); ctx.stroke(); }
    if (mistakes > 3) { ctx.beginPath(); ctx.moveTo(120, 140); ctx.lineTo(140, 180); ctx.stroke(); }
    if (mistakes > 4) { ctx.beginPath(); ctx.moveTo(120, 100); ctx.lineTo(90, 120); ctx.stroke(); }
    if (mistakes > 5) { ctx.beginPath(); ctx.moveTo(120, 100); ctx.lineTo(150, 120); ctx.stroke(); }
}

function updateDisplay() {
    const displayWord = currentWord
        .split("")
        .map(letter => guessedLetters.includes(letter) ? letter : "_")
        .join(" ");
    wordDisplay.textContent = displayWord;

    questionDiv.textContent = `Подсказка: ${currentQuestion}`;

    if (isAltView) {
        usedLettersDiv.innerHTML = "Использовано: " + (guessedLetters.length
            ? guessedLetters.map(letter => {
                const color = currentWord.includes(letter) ? "lightgreen" : "indianred";
                return `<span style="color: ${color}; font-weight: bold;">${letter}</span>`;
            }).join(", ")
            : "—");

        remainingDiv.textContent = `Осталось попыток: ${maxMistakes - mistakes}`;
    }
}

function handleLetterClick(button, letter) {
    guessedLetters.push(letter);

    if (currentWord.includes(letter)) {
        button.classList.add("correct");
    } else {
        button.classList.add("wrong");
        mistakes++;
        if (!isAltView) drawHangman(mistakes);
    }

    updateDisplay();
    checkGameStatus();
}

function checkGameStatus() {
    const allGuessed = currentWord.split("").every(l => guessedLetters.includes(l));
    if (allGuessed) {
        message.textContent = "Вы выиграли";
        disableKeyboard();
        restartBtn.style.display = "inline-block";
    } else if (mistakes >= maxMistakes) {
        message.textContent = `Вы проиграли. Загаданное слово: ${currentWord}`;
        disableKeyboard();
        restartBtn.style.display = "inline-block";
    }
}

function disableKeyboard() {
    keyboard.querySelectorAll("button").forEach(btn => btn.disabled = true);
}

function createKeyboard() {
    keyboard.innerHTML = "";
    for (let i = 1040; i <= 1071; i++) {
        const letter = String.fromCharCode(i);
        const button = document.createElement("button");
        button.textContent = letter;
        button.addEventListener("click", () => handleLetterClick(button, letter));
        keyboard.appendChild(button);
    }
}

function startGame() {
    const random = words[Math.floor(Math.random() * words.length)];
    currentWord = random.word.toUpperCase();
    currentQuestion = random.question;
    guessedLetters = [];
    mistakes = 0;
    message.textContent = "";
    restartBtn.style.display = "none";

    if (!isAltView) drawHangman(0);
    createKeyboard();

    updateDisplay();
}

restartBtn.addEventListener("click", startGame);

fetch("words.txt")
    .then(response => response.text())
    .then(text => {
        words = text
            .split("\n")
            .map(line => line.trim())
            .filter(line => line.includes(";"))
            .map(line => {
                const [word, question] = line.split(";");
                return {
                    word: word.toLowerCase(),
                    question: question.trim()
                };
            });
        startGame();
    });
