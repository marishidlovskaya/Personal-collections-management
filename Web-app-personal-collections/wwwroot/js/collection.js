$(document).ready(function () {
    var category = 0;
    $.ajax({
        url: "/Home/GetAllCategories",
        type: "GET",
        dataType: "json",
        success: function (data) {
            var classStyle = '';
            for (var i = 0; i < data.length; i++) {
                if (i == 0) {
                    classStyle = 'my-4 ms-0 me-4"';
                }
                else if (i == data.length - 1) {
                    classStyle = 'my-4 ms-4 me-0"';
                }
                else {
                    classStyle = 'm-4"';
                }
                $(".categories").append($('<div id="' + data[i].id + '" class="icon-element  ' + classStyle + '><img src="/images/' + data[i].name + '.svg" width="40" height="40">' + '<p class="icon-text">' + data[i].name + '</p></div>'));
            }
            $('.icon-element').click(function () {
                var id = $(this).prop('id');
                var ordering = 'bycategory';
                category = id;
                SortTable(category, ordering);
                $('.icon-text').css("text-decoration", "none");
                $('#' + id + ' p').css("text-decoration", "underline");
            })
        }
    });
    $.ajax({
        url: "/Home/GetAllCollections",
        type: "GET",
        dataType: "json",
        success: function (data) {
            InitCollectionTable(data);
        }
    });

    /*home page table*/
    function InitCollectionTable(data) {
        $('#collectionstable').DataTable(
            {
                data: data,
                searching: false,
                "bFilter": false,
                "bInfo": false,
                "bLengthChange": false,
                "bSort": false,
                "columns": [
                    { data: 'id' },
                    {
                        "width": "20%",
                        "render": function (data, type, row, meta) {
                            return '<img src="' + row.image + '"id="' + row.id + '" class="img-table" style="width: 200px;' +
                                'height: 200px; object-fit: cover; ">'
                        }
                    },
                    {
                        "width": "50%",
                        "render": function (data, type, row, meta) {
                            return '<div id="' + row.id + '"class="collection-name mb-3">' + row.name + '</div>' +
                                '<div class="col-description">' + row.description + '</div>' +
                                '<div class="text-muted mt-2">' + row.numberOfLikes + " likes" + '</div>' +
                                '<div>' + row.tags.map(s => '<a class="tag-name" href="#">' + '#' + s.name + '</a>').join(' ') +
                                '</div>'
                        }
                    }
                ],
                columnDefs: [
                    {
                        target: 0,
                        visible: false,
                        searchable: true,
                    },
                ],
                "drawCallback": function (settings) {
                    $('.collection-name, .img-table').on("click", function () {
                        var id = $(this).prop('id');
                        document.location.href = '/Collection/Index?id=' + id;
                    })
                    $('.tag-name').on("click", function () {
                        var tag = $(this).text();
                        $('#searchModal').modal('show');
                        $('#searchmodalblock').val(tag);
                        $.ajax({
                            url: "/Home/GetSearchResult",
                            type: "GET",
                            dataType: "json",
                            data: { searchInput: tag },
                            success: function (data) {
                                $('#resulttable').DataTable().clear().rows.add(data).draw();
                            }
                        });
                    })
                }
            }
        )
    }
    const queryString = window.location.search;
    const urlParams = new URLSearchParams(queryString);
    const id = urlParams.get('id');
    $.ajax({
        url: "/Collection/GetAllItemsByColId",
        type: "GET",
        dataType: "json",
        data: { id: id },
        success: function (data) {
            for (var i = 0; i < data.items.length; i++) {
                if (data.items[i].date1 != null) data.items[i].date1 = new Date(data.items[i].date1).toJSON().split("T")[0];
                if (data.items[i].date2 != null) data.items[i].date2 = new Date(data.items[i].date2).toJSON().split("T")[0];
                if (data.items[i].date3 != null) data.items[i].date3 = new Date(data.items[i].date3).toJSON().split("T")[0];
            }
            InitItemsTable(data);
        }
    });
    function InitItemsTable(data) {
        var col = data.columns;
        col[0] = {
            "width": "20%",
            "render": function (data, type, row, meta) {
                return '<img src="' + row.image + '"class="" style="width: 100px;' +
                    'height: 100px; object-fit: cover; ">'
            }
        };
        $('#itemstable').DataTable(
            {
                data: data.items,
                "bLengthChange": false,
                "language": {
                    "search": "Search item",
                    "emptyTable": "No items available in collection"
                },
                "columns": col
            }
        )
    }
    $('#sortingoptions').change(function () {
        var ordering = $('#sortingoptions').val();
        SortTable(category, ordering);
    })
    function SortTable(catId, ordering) {
        $.ajax({
            url: "/Home/GetAllCollections",
            type: "GET",
            dataType: "json",
            data: { catId: catId, ordering: ordering },
            success: function (data) {
                $('#collectionstable').DataTable().clear().rows.add(data).draw();
            }
        });
    }
    $("#addCommentBtn").on("click", function (e) {
        const queryString = window.location.search;
        const urlParams = new URLSearchParams(queryString);
        const id = urlParams.get('id');
        var comment = $('#addANote').val();
        $.ajax({
            url: "/Collection/AddComment",
            type: "POST",
            dataType: "json",
            data: { id: id, comment: comment },
            success: function (data) {
                var date = new Date(data.dateTimeOfComment);
                var dateFormatted = date.toLocaleDateString('en-US', {
                    day: '2-digit',
                    month: '2-digit',
                    year: 'numeric',
                    hour: '2-digit',
                    minute: '2-digit',
                    second: '2-digit',
                    hour12: false,
                    timeZone: 'UTC'
                }).replace(/(\d+)\/(\d+)\/(\d+)/, '$2.$1.$3').replace(',', '');

                $("#addANote").val('');

                var $div = '<div class="card shadow-0 border bg-light comment">' +
                    '<div class= "card-body p-4">' +
                    '<div class="card mb-4">' +
                    '<div class="card-body"><p class="comment-text-style">' + data.text +
                    '</p> <div class="mt-2 d-flex justify-content-between">' +
                    '<div class="d-flex flex-row align-items-center">' +
                    '<p class="small mb-0 ms-0">' + data.userName + '</p></div>' +
                    '<div class="d-flex flex-row align-items-center">' +
                    '<p class="small text-muted mb-0">' + dateFormatted + '</p></div></div></div></div></div></div>';
                $(".append-to-this").prepend($div);
            }
        });
    })
    $('.btn-mycollections').on("click", function () {

        document.location.href = '/ManageCollection/Index';
    })
})