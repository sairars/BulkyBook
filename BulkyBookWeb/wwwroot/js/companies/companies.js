$(document).ready(function () {
    CompaniesController.init("#Companies");
});

let CompaniesController = function () {
    let container;
    let table;

    let init = function (ctr) {
        container = $(ctr);

        loadDataTable();

        container.on("click", ".js-delete", confirmDeleteCompany);
    };

    let loadDataTable = function () {
        table = container.DataTable({
            ajax: {
                url: '/admin/api/companies',
                dataSrc: ''
            },
            columns: [
                { data: 'name' },
                { data: 'phoneNumber' },
                { data: 'streetAddress' },
                { data: 'city' },
                { data: 'state' },
                { data: 'postalCode' },
                {
                    data: "id",
                    render: function (data) {
                        return `<div class="btn-group w-75" role="group">
                                <a href="/Admin/Companies/Edit/${data}" class="btn btn-primary mx-2" role="button">
                                    <i class="bi bi-pencil-square me-2"></i>Edit
                                </a>
                                <a class="btn btn-danger mx-2 js-delete" data-company-id="${data}" role="button">
                                    <i class="bi bi-trash me-2"></i>Delete
                                </a>
                            </div>`;
                    }
                }
            ]
        });
    };

    let confirmDeleteCompany = function () {
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
                deleteCompany($(this));
            }
        })
    };

    let deleteCompany = function (button) {
        let companyId = button.attr("data-company-id");
        $.ajax({
            url: "/admin/api/companies/" + companyId,
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




