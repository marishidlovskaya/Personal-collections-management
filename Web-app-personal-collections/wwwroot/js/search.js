$(document).ready(function () {
    $('#searchblock').keyup(function () {
        OpenModalWindow()
    })
    $('#searchmodalblock').keyup(function () {
        GetSearchData($('#searchmodalblock').val());
    })
    var myModalEl = document.getElementById('searchModal')
    myModalEl.addEventListener('hidden.bs.modal', function (event) {
        $('#searchblock').val('');

    })
    $("#searchModal").on("shown.bs.modal", function () {
        $("#searchmodalblock").trigger("focus");
    });
    function OpenModalWindow() {
        var searchInput = $('#searchblock').val();
        if (searchInput) {
            $('#searchModal').modal('show');
            GetSearchData(searchInput);
        }
    }
    function GetSearchData(searchInput) {
        $('#searchmodalblock').val(searchInput);
        $.ajax({
            url: "/Home/GetSearchResult",
            type: "GET",
            dataType: "json",
            data: { searchInput: searchInput },
            success: function (data) {
                $('#resulttable').DataTable().clear().rows.add(data).draw();
            }
        });
    }
    function GetOnlyTagsSearchData(searchInput) {
        $('#searchmodalblock').val(searchInput);
        $.ajax({
            url: "/Home/GetOnlyTagsSearchResult",
            type: "GET",
            dataType: "json",
            data: { searchInput: searchInput },
            success: function (data) {
                $('#resulttable').DataTable().clear().rows.add(data).draw();
            }
        });
    }
    /* result of search */
    InitTable(null);
    function InitTable(data) {
        $('#resulttable').DataTable(
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
                        "render": function (data, type, row, meta) {
                            return '<img src="' + row.image + '" class="img-searh-table" style="width: 200px; height: 200px; object-fit: cover; ">'
                        }
                    },
                    {
                        "render": function (data, type, row, meta) {
                            return '<div class="text-muted mb-2">' + "Category: " + row.categoryName + '</div>' +
                                '<div id="' + row.id + '"class="col-Name-search mb-2">' + row.collectionName + '</div>' +
                                '<div>' +
                                row.tags.map(s => '<a class="tag-name-search" href="#">' + '#' + s + '</a>').join(' ')
                                + '</div>'
                        }
                    },
                    {
                        "render": function (data, type, row, meta) {
                            var numberOfComments = row.comments.length;
                            if (numberOfComments > 0) {
                                return '<div>' + "Result was found in " +
                                    '<span id="' + row.id + '"class="com-name-search">' +
                                    numberOfComments + " comments" + '</span></div>'
                            }
                            return '<div></div>'
                        }
                    },
                ],
                columnDefs: [
                    {
                        target: 0,
                        visible: false,
                        searchable: true,
                    },
                ],
                "drawCallback": function (settings) {
                    $('.col-Name-search').on("click", function () {
                        var id = $(this).prop('id');
                        document.location.href = '/Collection/Index?id=' + id;
                    })
                    $('.com-name-search').on("click", function () {
                        var id = $(this).prop('id');
                        document.location.href = '/Collection/Index?id=' + id;
                    })
                    $('.tag-name-search').on("click", function () {

                        var tag = "#" + $(this).text();
                        var newTag = tag.substring(1);
                        console.log(newTag);
                        GetOnlyTagsSearchData(newTag);
                    })
                }
            });
    }
    $(".emptylike").on("click", function () {
        $('#fulllike').off('click');
        $.ajax({
            url: "/Collection/CheckIfUserLogIn",
            type: "GET",
            dataType: "json",
            data: {},
            success: function (data) {
                $(".emptylike").attr('hidden', true)
                $(".fulllike").attr('hidden', false);
                const queryString = window.location.search;
                const urlParams = new URLSearchParams(queryString);
                const id = urlParams.get('id')
                $.ajax({
                    url: "/Collection/PutLikeOrDislike",
                    type: "POST",
                    dataType: "json",
                    async: false,
                    data: { id: id, isLike: true },
                    success: function (data) {
                        $('#fulllike').on('click');
                        $("#numLikes").load(" #numLikes");
                    }
                });
            },
            error: function (data) {
                $('#myModalError').modal('show');
            }
        })
    })

    $(".fulllike").on("click", function () {
        $('#emptylike').off('click');
        $.ajax({
            url: "/Collection/CheckIfUserLogIn",
            type: "GET",
            dataType: "json",
            data: {},
            success: function (data) {
                $(".emptylike").attr('hidden', false)
                $(".fulllike").attr('hidden', true);
                const queryString = window.location.search;
                const urlParams = new URLSearchParams(queryString);
                const id = urlParams.get('id')
                $.ajax({
                    url: "/Collection/PutLikeOrDislike",
                    type: "POST",
                    dataType: "json",
                    async: false,
                    data: { id: id, isLike: false },
                    success: function (data) {
                        $('#emptylike').on('click');
                        $("#numLikes").load(" #numLikes");
                    },
                })
            }
        })
    })
})










