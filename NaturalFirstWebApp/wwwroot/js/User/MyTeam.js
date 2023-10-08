$(document).ready(function () {
    $('#btnTen').click();
});

$('#btnTen').click(function () {
    $('#TenPercTbl').show();    
    $('#FivePercTbl').hide();    
    $('#ThreePercTbl').hide();
    $('#btnTen').addClass('valid-button');
    $('#btnTen').removeClass('invalid-button');
    $('#btnFive').removeClass('valid-button');
    $('#btnFive').addClass('invalid-button');
    $('#btnThree').removeClass('valid-button');
    $('#btnThree').addClass('invalid-button');
    GetMyTeam(10);
    $("#FivePercTbl tbody").empty();
    $("#ThreePercTbl tbody").empty();
});

$('#btnFive').click(function () {
    $('#TenPercTbl').hide();
    $('#FivePercTbl').show();
    $('#ThreePercTbl').hide();
    $('#btnTen').removeClass('valid-button');
    $('#btnTen').addClass('invalid-button');
    $('#btnFive').addClass('valid-button');
    $('#btnFive').removeClass('invalid-button');
    $('#btnThree').removeClass('valid-button');
    $('#btnThree').addClass('invalid-button');
    GetMyTeam(5);
    $("#TenPercTbl tbody").empty();
    $("#ThreePercTbl tbody").empty();
});

$('#btnThree').click(function () {
    $('#TenPercTbl').hide();
    $('#FivePercTbl').hide();
    $('#ThreePercTbl').show();
    $('#btnTen').removeClass('valid-button');
    $('#btnTen').addClass('invalid-button');
    $('#btnFive').removeClass('valid-button');
    $('#btnFive').addClass('invalid-button');
    $('#btnThree').addClass('valid-button');
    $('#btnThree').removeClass('invalid-button');
    GetMyTeam(3);
    $("#TenPercTbl tbody").empty();
    $("#FivePercTbl tbody").empty();
});




function GetMyTeam(percent) {
    let tbl = '';
    let tableHide1;
    let tableHide2;
    if (percent === 10) {
        tbl = 'TenPercTbl';
        tableHide1 = $('#FivePercTbl').DataTable();
        tableHide2 = $('#ThreePercTbl').DataTable();
    } else if (percent === 5) {
        tbl = 'FivePercTbl';
        tableHide1 = $('#TenPercTbl').DataTable();
        tableHide2 = $('#ThreePercTbl').DataTable();
    } else if (percent === 3) {
        tbl = 'ThreePercTbl';
        tableHide1 = $('#FivePercTbl').DataTable();
        tableHide2 = $('#TenPercTbl').DataTable();
    }    

    tableHide1.destroy();
    tableHide2.destroy();


    $.ajax({
        type: "Get",
        contentType: "application/json; charset=utf-8",
        url: '/User/GetMyTeam?perc=' + percent,//apiurl + 'api/PatchApi/GetMissing',
        //data: { perc: percent },
        dataType: "json",
        success: function (data) {
            $('#' + tbl).show();
            var table;
            if ($.fn.dataTable.isDataTable('#'+tbl+'')) {
                table = $('#' + tbl + '').DataTable().columns.adjust();
            } else {
                table = $('#' + tbl + '').DataTable();
            }
            table.destroy();
            $('#' + tbl + '').DataTable({
                data: data,
                paging: true,
                pagingType: "simple",
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
                        data: 'email',
                        maxWidth: '20px'
                    },
                    {
                        data: 'createdDate',
                        render: function (createdDate) {
                            return moment(createdDate).format('DD-MM-YYYY HH:mm');
                        }
                    },
                    {
                        data: 'userId',
                        render: function (userId) {
                            return '<a href="/Product/MyTeamProducts?userId=' + userId +'" style="text-decoration:none;"><button class="view-product-button" data-product="Product B">View</button></a>';
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
}