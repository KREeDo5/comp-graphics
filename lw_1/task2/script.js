function dragAndDrop(id) {
    let startX = 0, startY = 0, currentX = 0, currentY = 0;

    const trolleybus = document.getElementById(id);

    trolleybus.addEventListener('mousedown', (e) => {
        startX = e.clientX - currentX;
        startY = e.clientY - currentY;

        document.addEventListener('mousemove', onMouseMove);
        document.addEventListener('mouseup', onMouseUp);
    });

    function onMouseMove(e) {
        currentX = e.clientX - startX;
        currentY = e.clientY - startY;

        trolleybus.style.transform = `translate(${currentX}px, ${currentY}px)`;
    }

    function onMouseUp() {
        document.removeEventListener('mousemove', onMouseMove);
        document.removeEventListener('mouseup', onMouseUp);
    }
}

dragAndDrop('trolleybus1');
dragAndDrop('trolleybus2');
