$(document).ready(function () {

    //Maintenanace Table
    var table = $('#maintenance-table').DataTable({
        searching: true,
        paging: true,
        pageLength: 10,
        info: false,
        responsive: false,
        //dom: 'p', // Remove other built in buttons like pagination and search
        columnDefs: [
            { responsivePriority: 2, targets: 5 }, // Lower priority for the first column
            { responsivePriority: 1, targets: 1 } // Higher priority for the last column
        ],
        "dom": '<"row"<"col-sm-12"tr>>' +
            '<"row"<"col"p>>',
        layout: {
            bottomStart: '',
            topStart: null,
            topEnd: null,
        }
    });

    //Maintenance History Table of a hardware
    var tableHistory = $('#maintenance-history-table').DataTable({
        searching: true,
        paging: true,
        pageLength: 10,
        info: false,
        responsive: true,
        //dom: 'p', // Remove other built in buttons like pagination and search
        columnDefs: [
            { responsivePriority: 2, targets: 5 }, // Lower priority for the first column
            { responsivePriority: 1, targets: 1 } // Higher priority for the last column
        ],
        "dom": '<"row"<"col-sm-12"tr>>' +
            '<"row"<"col"p>>',
        layout: {
            bottomStart: '',
            topStart: null,
            topEnd: null,
        }
    });

    //Custom search box
    $('#custom-search-box').on('keyup', function () {
        table.search(this.value).draw();
    });
});