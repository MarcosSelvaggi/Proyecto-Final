function previewImagen(input) {
    var preview = document.getElementById("previewFoto");
    var lblIniciales = document.querySelector("[id$='lblIniciales']");
    var imgActual = document.querySelector("[id$='imgFotoActual']");

    if (input.files && input.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            preview.src = e.target.result;
            preview.style.display = "inline-block";
            if (lblIniciales) lblIniciales.style.display = "none";
            if (imgActual) imgActual.style.display = "none";
        };
        reader.readAsDataURL(input.files[0]);
    }
}