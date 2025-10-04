function openFileDialog() {
    document.getElementById('file-input').click();
}

function handleFileSelect(event) {
    const file = event.target.files[0];
    if (file) {
        const reader = new FileReader();
        reader.onload = function(e) {
            updateImage(e.target.result);
        };
        reader.readAsDataURL(file);
    }
}

function clearCanvas() {
    const canvas = document.getElementById('drawing');
    const ctx = canvas.getContext('2d');
    ctx.clearRect(0, 0, canvas.width, canvas.height);
}

function updateImage(src) {
    const img = document.getElementById('image');
    img.src = src;
    img.onload = function () {
        const imageContainer = document.getElementById('image-container');
        imageContainer.style.width = img.width + "px";
        imageContainer.style.height = img.height + "px";

        if (src.startsWith("data:image/png")) {
            imageContainer.style.backgroundImage = `
                linear-gradient(45deg, #ccc 25%, transparent 25%, transparent 75%, #ccc 75%),
                linear-gradient(45deg, #ccc 25%, transparent 25%, transparent 75%, #ccc 75%)`;
            imageContainer.style.backgroundSize = "20px 20px";
            imageContainer.style.backgroundPosition = "0 0, 10px 10px";
        } else {
            imageContainer.style.backgroundColor = "white";
        }

        updateCanvasSize()
        clearCanvas();
    };
}

function newButtonClick() {
    const imageContainer = document.getElementById('image-container');
    imageContainer.style.width = 80 + "vw";
    imageContainer.style.height = 80 + "vh";
    imageContainer.style.backgroundImage = 'none';

    const img = document.getElementById('image');
    img.src = "";
    clearCanvas();
}

function drawing() {
    const canvas = document.getElementById("drawing");
    const ctx = canvas.getContext("2d");
    const imageContainer = document.getElementById("image-container");
    canvas.width = imageContainer.clientWidth;
    canvas.height = imageContainer.clientHeight;

    let drawing = false;
    let lastX = 0, lastY = 0;
    const colorPicker = document.getElementById("color-picker");

    function startDrawing(e) {
        drawing = true;
        lastX = e.offsetX;
        lastY = e.offsetY;
    }

    function draw(e) {
        if (!drawing) {
            return;
        }
        ctx.strokeStyle = colorPicker.value;
        ctx.lineWidth = 10;
        ctx.lineCap = "round";
        ctx.lineJoin = "round";

        ctx.beginPath();
        ctx.moveTo(lastX, lastY);
        ctx.lineTo(e.offsetX, e.offsetY);
        ctx.stroke();

        lastX = e.offsetX;
        lastY = e.offsetY;
    }

    function stopDrawing() {
        drawing = false;
    }

    canvas.addEventListener("mousedown", startDrawing);
    canvas.addEventListener("mousemove", draw);
    canvas.addEventListener("mouseup", stopDrawing);
    canvas.addEventListener("mouseout", stopDrawing);
}

function saveImage() {
    const canvas = document.getElementById("drawing");
    const img = document.getElementById("image");
    const format = document.getElementById("format-select").value;

    //console.log(format);

    const canvasToSave = document.createElement("canvas");
    const canvasToSaveCtx = canvasToSave.getContext("2d");

    //console.log(img.naturalWidth, img.naturalHeight);

    if (img.naturalWidth !== 0) {
        canvasToSave.width = img.naturalWidth;
        canvasToSave.height = img.naturalHeight;
    } else {
        canvasToSave.width = canvas.width;
        canvasToSave.height = canvas.height;
    }

    if (format === "bmp" || format === "jpg") {
        canvasToSaveCtx.fillStyle = "#FFFFFF";
        canvasToSaveCtx.fillRect(0, 0, canvasToSave.width, canvasToSave.height);
    }

    if (img.naturalWidth !== 0) {
        canvasToSaveCtx.drawImage(img, 0,0, canvasToSave.width, canvasToSave.height);
    }
    canvasToSaveCtx.drawImage(canvas, 0,0, canvasToSave.width, canvasToSave.height);

    const link = document.createElement("a");
    link.download = "image." + format;
    link.href = canvasToSave.toDataURL(`image/${format}`);
    link.click();
}

function updateCanvasSize() {
    const canvas = document.getElementById("drawing");
    const imageContainer = document.getElementById("image-container");
    canvas.width = imageContainer.clientWidth;
    canvas.height = imageContainer.clientHeight;
}

window.onload = function () {
    document.getElementById('open-button').addEventListener('click', openFileDialog);
    document.getElementById('file-input').addEventListener('change', handleFileSelect);
    document.getElementById('new-button').addEventListener('click', newButtonClick);
    document.getElementById("save-button").addEventListener("click", saveImage);

    window.addEventListener("resize", updateCanvasSize);
    updateCanvasSize();

    drawing();
}