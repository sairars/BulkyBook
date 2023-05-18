$(document).ready(function () {
    $("#orderDetails").on("click", ".js-validate-shipping", validateShippingInfo);
});


let validateShippingInfo = function () {
    console.log("I am here");
    let carrier = $("#carrier");
    let trackingNumber = $("#trackingNumber");

    let message = "";

    if (carrier.val() === "") {
        message = "Please enter Carrier";
    }
    else if (trackingNumber.val() === "") {
        message = "Please enter Tracking Number";
    }
    else {
        return true;
    }
 
    Swal.fire({
        icon: 'error',
        text: message
    });

    return false;
}