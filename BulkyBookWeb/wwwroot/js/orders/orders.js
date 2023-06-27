$(function () {
    OrdersController.init("#Orders");
});

let OrdersController = function () {
    let container;
    let init = function (ctr) {
        container = $(ctr);

        let url = window.location.search;

        let status = "all";

        if (url.includes("paymentpending"))
            status = "paymentpending";
        else if (url.includes("approved"))
            status = "approved";
        else if (url.includes("inprocess"))
            status = "inprocess";
        else if (url.includes("completed"))
            status = "completed";
        
        loadDataTable(status);
    };

    let loadDataTable = function (status) {
        container.DataTable({
            ajax: {
                url: `/admin/api/orders/${status}`,
                dataSrc: ''
            },
            columns: [
                { data: 'id' },
                { data: 'name' },
                { data: 'phoneNumber' },
                { data: 'user.email' },
                { data: 'status' },
                { data: 'total'},
                {
                    data: "id",
                    render: function (data) {
                        return `<div class="btn-group w-75" role="group">
                                <a href="/Admin/Orders/Details/${data}" class="btn btn-default" role="button">
                                    <i class="bi bi-pencil-square"></i>
                                </a>
                            </div>`;
                    }
                }
            ]
        });
    };

    return {
        init: init
    };
}();


