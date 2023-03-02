$(document).ready(function () {
    const queryString = window.location.search;
    const urlParams = new URLSearchParams(queryString);
    const id = urlParams.get('id')
    $.ajax({
        url: "/ManageCollection/GetAllCollectionsByUserId",
        type: "GET",
        dataType: "json",
        data: { userId: id },
        success: function (data) {
            InitTableListOfMyCollections(data);
        }
    });
    function InitTableListOfMyCollections(data) {
        $('#mycollectionstable').DataTable(
            {
                data: data,
                autoWidth: true,
                "bLengthChange": false,
                "bInfo": false,
                "columns": [
                    { data: 'id' },
                    {
                        "render": function (data, type, row, meta) {
                            return '<div id="' + row.id + '"class="collection-Name">' + row.collectionName + '</div>'
                        }
                    },
                    {
                        "render": function (data, type, row, meta) {
                            var date = new Date(row.dateTimeCollectionAdded);
                            var options = { year: 'numeric', month: '2-digit', day: '2-digit' };
                            return date.toLocaleDateString('en-US', options).replace(/\//g, '.');
                        }
                    },
                    {
                        data: 'numberOfLikes'
                    },
                    {
                        "render": function (data, type, row, meta) {
                            return '<button type="button" class="edit-button-class btn btn-primary" style="width: 80px;" data-bs-toggle="modal" data-id="' + row.id + '" data-bs-target="#AddCollectionModal">Edit</button> ' +
                                '<button type="button" class="delete-button-class btn btn-danger" style="width: 80px;" data-bs-toggle="modal" data-id="' + row.id + '" data-bs-target="#myModalDelete">Delete</button>'
                        }
                    }
                ],
                "drawCallback": function (settings) {
                    var table = $('#mycollectionstable').DataTable();
                    $('.edit-button-class').on("click", function () {
                        var table = $('#mycollectionstable').DataTable();
                        var row = $(this).parents('tr');
                        var id = table.row(row).data().id;
                        $.ajax({
                            url: "/ManageCollection/GetCollectionInfoById",
                            type: "GET",
                            dataType: "json",
                            data: { id: id },
                            success: function (data) {
                                FillInCollectionForm(data);
                                $('#collectionId').val(id);
                                $(".select2Tags").each(function (index, element) {
                                    $(this).select2({
                                        tags: true,
                                        width: "100%"
                                    });
                                });
                                $('.select2Tags').empty().trigger('change');
                                var option = data.tags.map(i => (
                                    new Option(i, i, true, true)
                                ));
                                for (var i = 0; i < option.length; i++) {
                                    $('.select2Tags').append(option[i]).trigger('change');
                                }
                                var selectedTags = data.tags;
                                $.ajax({
                                    url: "/ManageCollection/GetTags",
                                    type: "GET",
                                    dataType: "json",
                                    success: function (data) {
                                        for (var i = 0; i < data.length; i++) {
                                            if (!selectedTags.includes(data[i].text)) {
                                                let option = new Option(data[i].text, data[i].text, false, false);
                                                $('.select2Tags').append(option).trigger('change');
                                            }
                                        }
                                    }
                                });
                            }
                        });
                    })
                    $('.delete-button-class').on("click", function () {
                        var row = $(this).parents('tr');
                        var id = table.row(row).data().id;
                        $('#confirmdeletion').on("click", function () {
                            $.ajax({
                                url: "/ManageCollection/DeleteCollectionById",
                                type: "POST",
                                dataType: "json",
                                data: { id: id },
                                success: function (data) {
                                }
                            });
                            table.row(row).remove().draw();
                        })
                    })
                    $('.collection-Name, .img-table').on("click", function () {
                        var id = $(this).prop('id');
                        document.location.href = '/Collection/Index?id=' + id;
                    })
                }
            })
    }

    function FillInCollectionForm(data) {
        var customFields = JSON.parse(data.additionalFields);
        var id = data.id;
        if ($.fn.DataTable.isDataTable('#items-table')) {
            $('#items-table').DataTable().destroy();
            $('#items-table').empty();
        }
        GetItemsDataInArrayFormat(customFields, data.items)
        $('#categories').val(data.categoryName);
        $('#collectionName').val(data.collectionName);
        $('#collectiondescription').val(data.collectionDescription);
        $('#CollectionImage').val(data.image);
        $('#collectionTags').val(data.tags.join(" "));
        $('.row-custom-field').remove();
        var inputFields = $('.inputs').find('input').slice(2);
        HideDeletedFields(inputFields);
        if (customFields != null) PopulateCustomFields(customFields);
    }

    function GetItemsDataInArrayFormat(customFields, items) {
        for (var i = 0; i < items.length; i++) {
            if (items[i].date1 != null) items[i].date1 = new Date(items[i].date1).toJSON().split("T")[0];
            if (items[i].date2 != null) items[i].date2 = new Date(items[i].date2).toJSON().split("T")[0];
            if (items[i].date3 != null) items[i].date3 = new Date(items[i].date3).toJSON().split("T")[0];
        }
        var data = [];
        var columns = [
            { title: "Image" },
            { title: "Name" },
        ];
        columns[0] = {
            "width": "20%",
            "render": function (data, type, row, meta) {
                return '<img src="' + data + '"class="img-table" style="width:80px;' +
                    'height: 80px; object-fit: cover; ">'
            }
        };
        if (customFields != null) {
            for (var i = 0; i < customFields.length; i++) {
                columns.push({ title: customFields[i].title });
            }
        }
        for (var i = 0; i < items.length; i++) {
            data.push([items[i].image, items[i].name])
            if (customFields != null) {
                for (var j = 0; j < customFields.length; j++) {
                    data[i].push(items[i][customFields[j].data])
                }
            }
        }
        InitTableWithItems(data, columns);
    }

    function PopulateCustomFields(customFields) {
        var counter = 0;
        for (var i = 0; i < customFields.length; i++) {

            var $div = '<div class="row row-custom-field" id="' + (counter) + '"> ' +
                '<div class="col-4"> <div>' +
                '<select id="types" class="btn-sm dropdown-toggle"' +
                'aria-label="Default select example" style = "width: 130px" > ' +
                '<option>Text</option>' +
                '<option>Number</option>' +
                '<option>Bool</option>' +
                '<option>Date</option>' +
                '</select></div></div>' +
                '<div class="col-7">' +
                '<div class="pb-3">' +
                '<input type="text" id="" value="' + customFields[i].title + '">' +
                '</div></div><div class="col-1">' +
                '<button type="button" class="btn-delete">x</button>' +
                '</div ></div>';

            $("#addtitionalFieldsData").append($($div));
            $('#' + counter).children().children().find('select').val(customFields[i].data.substring(0, customFields[i].data.length - 1).capitalize());
            counter++;
        }
        ShowAddedFields(3);
    }

    Object.defineProperty(String.prototype, 'capitalize', {
        value: function () {
            return this.charAt(0).toUpperCase() + this.slice(1);
        },
        enumerable: false
    });


    function InitTableWithItems(data, columns) {

        if (columns == null) {
            columns = [
                { title: "Image" },
                { title: "Name" },
            ];
        }
        columns.push({
            "render": function (data, type, row, meta) {
                return '<button type="button" id="editItem" class="edit-button-class btn btn-primary" style="width: 80px;">Edit</button> ' +
                    '<button type="button" id="deleteItem"class="delete-button-class btn btn-danger" style="width: 80px;">Delete</button>'
            }
        })

        $('#items-table').DataTable(
            {
                "bInfo": false,
                "bLengthChange": false,
                "language": {
                    "search": "Search item",
                    "emptyTable": "No items available in collection"
                },
                data: data,
                columns: columns

            })
    }

    function SortInputsBasedOnOrder() {
        const container = $('.inputs');
        const items = $('.pb-3', container);
        items.sort(function (a, b) {
            return parseInt($(a).css('order')) - parseInt($(b).css('order'));
        });
        return items;
    }

    function IsInputFieldVisible(inputField) {
        return ($(inputField).parent().css('display') != 'none');
    }

    function GetInputData(inputField) {

        var valueToPush = $(inputField).val();
        if ($(inputField).is(":checkbox") && $(inputField).is(':checked')) {
            valueToPush = "Yes"
        }
        if ($(inputField).is(":checkbox") && !$(inputField).is(':checked')) {
            valueToPush = "No"
        }
        return valueToPush;
    }

    $('#items-table').on('click', '#deleteItem', function () {
        var table = $('#items-table').DataTable();
        var row = $(this).parents('tr');

        if ($(row).hasClass('child')) {
            table.row($(row).prev('tr')).remove().draw();
        } else {
            table
                .row($(this).parents('tr'))
                .remove()
                .draw();
        }
    });

    $('#items-table').on('click', '#editItem', function () {
        var table = $('#items-table').DataTable();
        var row = $(this).parents('tr');
        var rowData = table.row(row).data();
        selectedRow = table.row(row).index();
        $('#AddItemModal').modal("show");
        $('#saveExistingItemAfterChanging').css('display', 'inline');
        $('#saveChangesNewItem').css('display', 'none');
        $(".modal-body #itemId").val(selectedRow);

        var items = SortInputsBasedOnOrder();
        var inputFields = $('.inputs').find('input')
        var i = 0;
        $(items).each(function () {
            inputFields = $(this).find('input');
            if (IsInputFieldVisible(inputFields)) {
                let val = rowData[i];
                if ($(inputFields).attr('type') == 'date') {
                    val = new Date(val).toJSON().split("T")[0];
                }
                $(inputFields).val(val)
                i++;
            }
        })
    });

    $('#saveExistingItemAfterChanging').on('click', function () {

        var data = [];
        var items = SortInputsBasedOnOrder();
        var inputFields = $('.inputs').find('input')
        $(items).each(function () {
            inputFields = $(this).find('input');
            if (IsInputFieldVisible(inputFields)) {
                var val = GetInputData(inputFields);
                data.push(val);
            }
        })
        var id = $(".modal-body #itemId").val();
        $('#items-table').DataTable().row(id).data(data).draw();

        $(items).each(function () {
            inputFields = $(this).find('input');
            $(inputFields).val("");
        })
        $('#AddItemModal').modal('hide');
    })

    $('#saveChangesNewItem').on("click", function () {
        var data = [];
        var columns = [];
        var items = SortInputsBasedOnOrder();

        $(items).each(function () {
            var inputFields = $(this).find('input');
            if (IsInputFieldVisible(inputFields)) {
                var val = GetInputData(inputFields);
                data.push(val);
                var inputId = $(inputFields).attr('id');
                var columnName = $('label[for="' + inputId + '"]').text();
                columns.push({ "title": columnName })
            }
        })
        var numCols = $('#items-table').DataTable().columns().nodes().length;
        var table = $('#items-table').DataTable();
        var tableData = table.rows().data();
        var diff = data.length - numCols + 1;

        if (diff > 0) {
            var dataArr = [];
            for (var i = 0; i < tableData.length; i++) {
                dataArr.push(tableData[i]);

                for (var j = 0; j < diff; j++) {
                    dataArr[i].push('');
                }
            }

            $('#items-table').DataTable().destroy();
            $('#items-table').empty();
            InitTableWithItems(dataArr, columns);
        }

        $('#items-table').DataTable().row.add(data).draw(false);

        $('.inputs').find('input').each(function () {
            $(this).val("");
        })
        $('#AddItemModal').modal('hide');
    })

    $('#btn-add-collection').on("click", function () {
        $('#AddCollectionModal').modal('show');
        InitTableWithItems(null);
        $(".select2Tags").each(function (index, element) {
            $(this).select2({
                tags: true,
                width: "100%"
            });
        });
        $.ajax({
            url: "/ManageCollection/GetTags",
            type: "GET",
            dataType: "json",
            success: function (data) {
                for (var i = 0; i < data.length; i++) {

                    let option = new Option(data[i].text, data[i].text, false, false);
                    $('.select2Tags').append(option).trigger('change');
                }
            }
        });
    })

    $('#btnAddNewItem').on("click", function () {
        $('#AddItemModal').modal('show');
        $('#saveExistingItemAfterChanging').css('display', 'none');
        $('#saveChangesNewItem').css('display', 'inline');
    })

    $('#btnAddCustomField').on("click", function () {
        $('#AddCustomFieldsModal').modal('show');
    })

    $('#btnAddNewField').on("click", function () {
        var counter = 0;
        var TextCounter = 0;
        var NumberCounter = 0;
        var BoolCounter = 0;
        var DateCounter = 0;

        $('.row-custom-field').each(function () {
            var FieldType = $(this).find('#types').val();
            if (FieldType == 'Text') TextCounter++;
            if (FieldType == 'Number') NumberCounter++;
            if (FieldType == 'Bool') BoolCounter++;
            if (FieldType == 'Date') DateCounter++;
            $(this).attr("id", counter);
            counter++;
        })

        var $div = '<div class="row row-custom-field" id="' + (counter++) + '"> ' +
            '<div class="col-4"> <div>' +
            '<select id="types" class="btn-sm dropdown-toggle"' +
            'aria-label="Default select example" style = "width: 130px" > ' +
            '<option>Text</option>' +
            '<option>Number</option>' +
            '<option>Bool</option>' +
            '<option>Date</option>' +
            '</select></div></div>' +
            '<div class="col-7">' +
            '<div class="pb-3">' +
            '<input type="text" id="">' +
            '</div></div><div class="col-1">' +
            '<button type="button" class="btn-delete">x</button>' +
            '</div ></div>';

        if (TextCounter > 2) {
            $div = $div.replace('<option>Text</option>', '')
            RemoveOptions("Text")
        }
        if (NumberCounter > 2) {
            $div = $div.replace('<option>Number</option>', '')
            RemoveOptions("Number")
        }
        if (BoolCounter > 2) {
            $div = $div.replace('<option>Bool</option>', '')
            RemoveOptions("Bool")
        }
        if (DateCounter > 2) {
            $div = $div.replace('<option>Date</option>', '')
            RemoveOptions("Date")
        }
        if ((TextCounter + NumberCounter + BoolCounter + DateCounter) <= 11) {

            $("#addtitionalFieldsData").append($($div));
            $('.row-custom-field').each(function () {
                var FieldType = $(this).find('#types').val();
                if (FieldType == 'Text') TextCounter++;
                if (FieldType == 'Number') NumberCounter++;
                if (FieldType == 'Bool') BoolCounter++;
                if (FieldType == 'Date') DateCounter++;
            })
            if (TextCounter > 2) {
                $div = $div.replace('<option>Text</option>', '')
                RemoveOptions("Text")
            }
            if (NumberCounter > 2) {
                $div = $div.replace('<option>Number</option>', '')
                RemoveOptions("Number")
            }
            if (BoolCounter > 2) {
                $div = $div.replace('<option>Bool</option>', '')
                RemoveOptions("Bool")
            }
            if (DateCounter > 2) {
                $div = $div.replace('<option>Date</option>', '')
                RemoveOptions("Date")
            }
        }
        else {
            alert("No more than 12 custom fields");
        }
    })

    $("#addtitionalFieldsData").delegate(".row-custom-field", "focus", function () {
        var previousVal = $(this).find('#types').val();
        var divSelector = $(this).find('#types')[0];

        $(divSelector).change(function () {

            var newValue = this.value;

            var TextCounter = 0;
            var NumberCounter = 0;
            var BoolCounter = 0;
            var DateCounter = 0;

            $('.row-custom-field').each(function () {
                var FieldType = $(this).find('#types').val();
                if (FieldType == 'Text') TextCounter++;
                if (FieldType == 'Number') NumberCounter++;
                if (FieldType == 'Bool') BoolCounter++;
                if (FieldType == 'Date') DateCounter++;
            })

            $('.row-custom-field').each(function () {
                var divSelectorVal = $(this).find('#types').val();
                let select = $(this).find('#types')[0];
                let options = select.options
                let isExist = false;
                /*iterate through all options of current dropdown*/
                for (let i = 0; i < options.length; i++) {
                    if (options[i].value === previousVal) {
                        isExist = true;
                        break;
                    }
                }
                if (!isExist) {
                    $(this).find('#types')[0].append(new Option(previousVal));
                }
                if (newValue == 'Text' && TextCounter >= 2) {
                    RemoveOptions('Text');
                }
                if (newValue == 'Number' && NumberCounter >= 2) {
                    RemoveOptions('Number');
                }
                if (newValue == 'Bool' && BoolCounter >= 2) {
                    RemoveOptions('Bool');
                }
                if (newValue == 'Date' && DateCounter >= 2) {
                    RemoveOptions('Date');
                }
            })
        })
    });

    function HideDeletedFields(inputFields) {
        $(inputFields).each(function () {
            var name = $(this).attr('name');
            var inputId = $(this).attr('id');
            var label = $('label[for="' + inputId + '"]');
            var textFieldName = label.text();
            var isFieldExists = false;
            $('.row-custom-field').each(function () {
                var typeOfField = $(this).find('#types').val().toLowerCase();
                var nameOfField = $(this).find('input').val();
                if (typeOfField == name && nameOfField == textFieldName) {
                    isFieldExists = true;
                }
            })
            if (isFieldExists == false) {
                $(this).parent().css('display', 'none');
                label.parent().css('display', 'none');
                label.text("")
            }
        })
    }

    function ShowAddedFields(order) {

        $('.row-custom-field').each(function () {
            var inputFields = $('.inputs').find('input').slice(2);
            var typeOfField = $(this).find('#types').val();
            var nameOfField = $(this).find('input').val();
            var isUsed = false;

            for (var i = 0; i < inputFields.length; i++) {
                var idOfInput = $(inputFields[i]).attr('id');
                if (nameOfField == $('label[for="' + idOfInput + '"]').text()) {
                    isUsed = true;
                    SetOrder($(inputFields[i]), idOfInput, order);
                    order++;
                    break;
                }
            }
            for (var i = 0; i < inputFields.length; i++) {
                var idOfInput = $(inputFields[i]).attr('id');
                var labelName = $('label[for="' + idOfInput + '"]').text();
                var name = $(inputFields[i]).attr('name');

                if (typeOfField.toLowerCase() == name && labelName == nameOfField && IsInputFieldVisible(inputFields[i])) {
                    $($(inputFields[i])).parent().css('order', order);
                    $('label[for="' + idOfInput + '"]').parent().css('order', order)
                    order++;
                    break;
                }
                if ($(inputFields[i]).parent().css('display') == 'none' && name == typeOfField.toLowerCase() && !isUsed) {
                    $('label[for="' + idOfInput + '"]').text(nameOfField);
                    $($(inputFields[i])).parent().css('display', 'flex')
                    $('label[for="' + idOfInput + '"]').parent().css('display', 'flex')
                    $($(inputFields[i])).parent().css('order', order);
                    $('label[for="' + idOfInput + '"]').parent().css('order', order)
                    order++;
                    break;
                }
            }
        })
        return order;
    }

    function SetOrder(inputField, idOfInputField, order) {
        $(inputField).parent().css('order', order);
        $('label[for="' + idOfInputField + '"]').parent().css('order', order)
    }

    $("#AddCustomFieldsModal").delegate("#saveFields", "click", function () {
        var inputFields = $('.inputs').find('input').slice(2);
        var order = 3;
        HideDeletedFields(inputFields);
        order = ShowAddedFields(order);
        var items = SortInputsBasedOnOrder();
        var data = [];
        var columns1 = [];
        var inputFields = $('.inputs').find('input')

        $(items).each(function () {
            inputFields = $(this).find('input');
            if (IsInputFieldVisible(inputFields)) {
                data.push($(inputFields).val())
                var inputId = $(inputFields).attr('id');
                var columnName = $('label[for="' + inputId + '"]').text();
                columns1.push({ "title": columnName })
                order++;
            } else {
                $(inputFields).parent().css('order', order);
                $('label[for="' + inputId + '"]').parent().css('order', order);
                order++;
            }
        })
        var arrOfIDs = [];
        arrOfIDs.push(0);
        arrOfIDs.push(1);
        $('.row-custom-field').each(function () {
            var id = parseInt($(this).attr("id")) + 2;
            arrOfIDs.push(id);
        })
        var numCols = $('#items-table').DataTable().columns().nodes().length;
        var table = $('#items-table').DataTable();
        var tableData = table.rows().data();
        var diff = data.length - numCols + 1;
        var dataArr = [];
        if (diff != 0) {
            dataArr = [];
            for (var i = 0; i < tableData.length; i++) {
                dataArr.push(tableData[i]);

                for (var j = 0; j < diff; j++) {
                    dataArr[i].push('');
                }
            }
            $('#items-table').DataTable().destroy();
            $('#items-table').empty();
            if (diff < 0) {
                var newdataArr = [];
                for (var j = 0; j < dataArr.length; j++) {
                    var newrowArr = [];
                    for (var i = 0; i < arrOfIDs.length; i++) {
                        var id = arrOfIDs[i];
                        var val = dataArr[j][id];
                        newrowArr.push(val)
                    }
                    newdataArr.push(newrowArr);
                }
                InitTableWithItems(newdataArr, columns1);
            } else {
                InitTableWithItems(dataArr, columns1);
            }
        }
        $('#AddCustomFieldsModal').modal('toggle');
    })

    $("#addtitionalFieldsData").delegate(".btn-delete", "click", function () {
        var option = $(this).parent().parent().find('#types').val();
        var idOfRow = $(this).parent().parent().attr('id');
        $("#addtitionalFieldsData").children('#' + idOfRow).remove();
        $('.row-custom-field').each(function () {
            var divSelector = $(this).find('#types')[0];
            let options = divSelector.options;
            var isAvailable = false;
            for (let i = 0; i < options.length; i++) {
                if (options[i].value === option) {
                    isAvailable = true;
                    break;
                }
            }
            if (!isAvailable) {
                $(divSelector).append(new Option(option));
            }
        })
    });
    function RemoveOptions(option) {
        $('.row-custom-field').each(function () {
            var FieldType = $(this).find('#types').val();
            if (option != FieldType) {
                let select = $(this).find('#types')[0];
                let options = select.options
                for (let i = 0; i < options.length; i++) {
                    if (options[i].value === option) {
                        options[i].remove();
                        break;
                    }
                }
            }
        })
    }

    $('#saveNewCollection').on("click", function () {
        var data = GetAllCollectionData();
        var collectionId = $('#collectionId').val();
        $.ajax({
            url: "/ManageCollection/SaveCollection",
            type: "POST",
            dataType: "json",
            data: { data: JSON.stringify(data), collectionId: collectionId },
            success: function (data) {
            }
        });
        $('#AddCollectionModal').modal('toggle');
    })

    function GetCustomFields() {
        var fields = [];
        var inputFields = $('.inputs').find('input');
        $(inputFields).each(function () {
            if (IsInputFieldVisible(this)) {
                var inputId = $(this).attr('id');
                if (inputId != "image" && inputId != "name") {
                    fields.push({ data: inputId, title: $('label[for="' + inputId + '"]').text() })
                }
            }
        })
        return fields;
    }

    function GetAllCollectionData() {
        var object = {};
        object.CategoryName = $('#categories').val();
        object.CollectionName = $('#collectionName').val();
        object.CollectionDescription = $('#collectiondescription').val();
        object.Image = $('#CollectionImage').val();
        var items = [];
        $('#items-table').DataTable().rows().every(function (rowIdx, tableLoop, rowLoop) {
            var data = this.data();
            var item = {};
            var counter = 0;
            var items2 = SortInputsBasedOnOrder();
            $(items2).each(function () {
                var inputFields = $(this).find('input');
                if (IsInputFieldVisible(inputFields)) {
                    var inputId = $(inputFields).attr('id');
                    switch (inputId) {
                        case "name":
                            item.Name = data[counter]
                            break;
                        case "image":
                            item.Image = data[counter]
                            break;
                        case "text1":
                            item.Text1 = data[counter]
                            break;
                        case "text2":
                            item.Text2 = data[counter]
                            break;
                        case "text3":
                            item.Text3 = data[counter]
                            break;
                        case "number1":
                            item.Number1 = parseFloat(data[counter])
                            break;
                        case "number2":
                            item.Number2 = parseFloat(data[counter])
                            break;
                        case "number2":
                            item.Number3 = parseFloat(data[counter])
                            break;
                        case "bool1":
                            item.Bool1 = ConvertToBool(data[counter])
                            break;
                        case "bool2":
                            item.Bool2 = ConvertToBool(data[counter])
                            break;
                        case "bool3":
                            item.Bool3 = ConvertToBool(data[counter])
                            break;
                        case "date1":
                            item.Date1 = new Date(data[counter])
                            break;
                        case "date2":
                            item.Date2 = new Date(data[counter])
                            break;
                        case "date3":
                            item.Date3 = new Date(data[counter])
                            break;
                    }
                    counter++
                }
            })
            items.push(item);
        });
        object.Items = items;
        object.AdditionalFields = JSON.stringify(GetCustomFields())
        object.Tags = $('.select2Tags').val();
        return object;
    }
    function ConvertToBool(value) {
        if (value === "Yes") return true;
        if (value === "No") return false;
        return null;
    }
})

