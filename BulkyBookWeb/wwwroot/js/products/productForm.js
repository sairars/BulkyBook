$(function () {
    $("#productForm").on("click", ".js-validate-upload", validateFileUpload);
});

let validateFileUpload = function () {
    console.log("Hello");
    let uploadBox = $("#uploadBox");

    if (uploadBox.val() !== "") {
        return true;
    }

    Swal.fire({
        icon: 'error',
        text: 'Please upload an image!'
    });

    return false;
}

tinymce.init({
    selector: 'textarea',
    plugins: 'autolink charmap  lists wordcount',
    toolbar: 'undo redo | blocks fontfamily fontsize | bold italic underline strikethrough | align lineheight | numlist bullist indent outdent',
    elementpath: false
});