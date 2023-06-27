$(function () {
    ProductsController.init("#Products");
});

let ProductsController = function () {
    let container;
    let table;

    let init = function (ctr) {
        container = $(ctr);

        loadDataTable();

        container.on("click", ".js-delete", confirmDeleteProduct);
    };

    let loadDataTable = function () {
        table = container.DataTable({
            ajax: {
                url: '/admin/api/products',
                dataSrc: ''
            },
            columns: [
                { data: 'title' },
                { data: 'isbn' },
                { data: 'price' },
                { data: 'author' },
                { data: 'category.name' },
                {
                    data: "id",
                    render: function (data) {
                        return `<div class="btn-group w-75" role="group">
                                <a href="/Admin/Products/Edit/${data}" class="btn btn-primary mx-2" role="button">
                                    <i class="bi bi-pencil-square me-2"></i>Edit
                                </a>
                                <a class="btn btn-danger mx-2 js-delete" data-product-id="${data}" role="button">
                                    <i class="bi bi-trash me-2"></i>Delete
                                </a>
                            </div>`;
                    }
                }
            ]
        });
    };

    let confirmDeleteProduct = function () {
        Swal.fire({
            title: 'Are you sure?',
            text: "You won't be able to revert this!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, delete it!'
        }).then((result) => {
            if (result.isConfirmed) {
                deleteProduct($(this));
            }
        })
    };

    let deleteProduct = function (button) {
        let productId = button.attr("data-product-id");
        $.ajax({
            url: "/admin/api/products/" + productId,
            method: "DELETE"
        })
            .done(function (data) {
                table.ajax.reload();
                toastr.success(data);
            })
            .fail(function (data) {
                toastr.error(data);
            });
    }

    return {
        init: init
    };
}();


