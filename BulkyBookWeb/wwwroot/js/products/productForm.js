$(document).ready(function () {
    $("#productForm").on("click", ".js-validate-upload", validateFileUpload);
});


let validateFileUpload = function () {
    let uploadBox = $("#uploadBox");

    if (uploadBox.val() !== "") {
        return true;
    }

    Swal.fire({
        icon: 'error',
        title: 'Oops...',
        text: 'Please upload an image!'
    });

    return false;
}