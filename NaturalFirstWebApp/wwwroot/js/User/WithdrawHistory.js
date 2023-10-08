$(document).ready(function () {
    $.ajax({
        type: "Get",
        contentType: "application/json; charset=utf-8",
        url: '/User/GetHistoryWithdrawal',//apiurl + 'api/PatchApi/GetMissing',
        //data: josnstr,
        dataType: "json",
        success: function (data) {
            var table;
            if ($.fn.dataTable.isDataTable('#WithdrawHistoryTbl')) {
                table = $('#WithdrawHistoryTbl').DataTable().columns.adjust();
            } else {
                table = $('#WithdrawHistoryTbl').DataTable();
            }
            table.destroy();
            $("#WithdrawHistoryTbl").DataTable({
                data: data,
                paging: true,
                pagingType:"simple",
                sort: false,
                searching: true,
                ordering: true,
                order: [],
                lengthMenu: [
                    [10, 25, 50, -1],
                    ['10 Rows', '25 Rows', '50 Rows', 'Show All']
                ],
                responsive: false,
                columns: [
                    //{
                    //    data: 'Id',
                    //    sWidth: '2px',
                    //    sClass: "view",
                    //    bSortable: false,
                    //    render: function (Id) {
                    //        return '<input id="check" class="cb-element servcbk" name="' + Id + '" type="checkbox">';;
                    //    }
                    //},
                    {
                        data: 'amount',
                        maxWidth: '20px'
                    },
                    {
                        data: 'createdDate',
                        render: function (createdDate) {
                            return moment(createdDate).format('DD-MM-YYYY HH:mm');
                        }
                    },
                    {
                        data: 'status',
                        render: function (status) {
                            if (status === 0) {
                                return 'Pending'
                            }
                            if (status === 1) {
                                return 'Success'
                            }
                            if (status === 2) {
                                return 'Failed'
                            }
                        }
                    },
                ],
                // dom: 'Bfrtip',
                dom: 'Bflrtip',
                //buttons: [
                //    {
                //        extend: 'copyHtml5',
                //        text: '<i class="far fa-file fa-1x" style="color:blue"></i>',
                //        titleAttr: 'Copy'
                //    },
                //    {
                //        extend: 'excelHtml5',
                //        text: '<i class="far fa-file-excel fa-1x" style="color:#23af73"></i>',
                //        titleAttr: 'Excel'
                //    },
                //    {
                //        extend: 'pdfHtml5',
                //        text: '<i class="far fa-file-pdf fa-1x" style="color:#f64e60"></i>',
                //        titleAttr: 'PDF'
                //    }
                //]
            });
        },

        error: function (edata) {
            alert("Error while fetching records.");
        }
    });
});