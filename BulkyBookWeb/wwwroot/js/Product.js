$(document).ready(function () {
    loadDataTable();
    //console.log("result");
});

function loadDataTable() {
    let table = $("#Products").DataTable({
        ajax: {
            url: '/Admin/Products/GetAllProducts',
            dataSrc: 'productsData'
        },
        columns: [
            { data: 'title'},
            { data: 'isbn'},
            { data: 'price'},
            { data: 'author' }
        ]
    });    
}


