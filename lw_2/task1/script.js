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

function updateImage(src) {
    const img = document.getElementById('image');
    img.src = src;
    img.onload = function () {
        resizeImage();
    };
}

function resizeImage() {
    const img = document.getElementById("image");
    const container = document.getElementById("image-container");


    const containerWidth = container.clientWidth;
    const containerHeight = container.clientHeight;

    const imgWidth = img.naturalWidth;
    const imgHeight = img.naturalHeight;

    const imgDiff = imgWidth / imgHeight;
    const containerDiff = containerWidth / containerHeight;

    if (imgDiff > containerDiff) {
        img.style.width = "100%";
        img.style.height = "auto";

        container.style.width = img.width + "px";
        container.style.height = (img.width / imgDiff) + "px";
    } else {
        img.style.width = "auto";
        img.style.height = "100%";

        container.style.height = img.height + "px";
        container.style.width = (img.height * imgDiff) + "px";
    }
}

function dragAndDrop(id) {
    let startX = 0, startY = 0, currentX = 0, currentY = 0;

    const image = document.getElementById(id);

    image.addEventListener('mousedown', mouseDown);

    function mouseDown(e) {
        e.preventDefault();
        startX = e.clientX - currentX;
        startY = e.clientY - currentY;

        document.addEventListener('mousemove', mouseMove);
        document.addEventListener('mouseup', mouseUp);
    }

    function mouseMove(e) {
        currentX = e.clientX - startX;
        currentY = e.clientY - startY;

        image.style.transform = `translate(${currentX}px, ${currentY}px)`;
    }

    function mouseUp() {
        document.removeEventListener('mousemove', mouseMove);
        document.removeEventListener('mouseup', mouseUp);
    }
}

window.onload = function (){
    document.getElementById('open-button').addEventListener('click', openFileDialog);
    document.getElementById('file-input').addEventListener('change', handleFileSelect);
    window.addEventListener("resize", resizeImage);

    dragAndDrop('image-container')
}