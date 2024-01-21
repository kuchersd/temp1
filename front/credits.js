$(function (){
    ShowEnvironmentLabel();
    window.webServiceURL = "https://localhost:7093";

    $("#addProCreditsButton").click(function () {
        ShowCreateProCreditsModal();
    });
});

$("#searchProCreditsButton").click(function (event) {
    var shopperId = document.querySelector("#inputShopperId").value;

    GetTableData(shopperId);
});

function ShowCreateProCreditsModal() {
    $('#createProCreditsModal').modal('show');

    var shopperId = document.querySelector("#inputShopperId").value;

    if (!!shopperId)
    {
        document.querySelector("#shopperIdField").value = shopperId;
    }
}

function getShopperIdFromURL() {
    var url = window.location.href;

    var parts = url.split("?");

    if (parts.length > 1) {
        var params = parts[1];

        var paramArray = params.split("&");

        for (var i = 0; i < paramArray.length; i++) {
            var param = paramArray[i].split("=");
            var key = param[0];
            var value = param[1];

            if (key === 'shopper_id') {
                GetTableData(value);
                document.querySelector("#inputShopperId").value = value;
            }
        }
    }
}
window.addEventListener('load', getShopperIdFromURL);

const ShopperProCreditSourceType = {
    0: 'Unknown',
    1: 'ProCreditPack3',
    2: 'ProPremiumMonthlyInitial',
    3: 'ProPremiumAnnualInitial',
    4: 'CustomerService',
    5: 'ProPremiumMonthlyRebill',
    6: 'ProPremiumAnnualRebill',
    7: 'ProPlusAnnualInitial',
    8: 'ProPlusAnnualUpgrade',
    9: 'ProPremiumMonthlyUpgrade',
    10: 'ProPremiumAnnualUpgrade',
    11: 'ContestWinner',
    12: 'ProCreditPackRefill3',
}

function GetTableData(shopperId) {
    
    var getAllProCreditsUrl = window.webServiceURL+ "/api/pro-credit/getbyshopperid?shopperid=" + shopperId;

    console.log(ShopperProCreditSourceType[3]);

    var dataTable = $('#creditsTable').DataTable({
        "ajax": { "url": getAllProCreditsUrl, "dataSrc": "" },
        "initComplete": function (settings, json) {
        },
        "columns": [
            { "data": null, "defaultContent": '<div><input type="checkbox" id="scales" name="scales" /></div>' },
            { "data": "shopperProCreditID" },
            { "data": "redemptionOrderId"},
            { "data": "dateExpired" },
            { "data": "shopperProCreditSourceTypeId" },
            { "data": "creditValue" }
        ],
        "columnDefs": [
            { targets: 3,  render: function convertDateFormat(inputDate) {
                const parsedDate = new Date(inputDate);
            
                const month = parsedDate.getMonth() + 1;
                const day = parsedDate.getDate();
                const year = parsedDate.getFullYear();
            
                const formattedDate = `${month.toString().padStart(2, '0')}/${day.toString().padStart(2, '0')}/${year}`;
            
                return formattedDate;
            } },
            { targets: 4, render: function convertType(inputType) {

                const updatedType = ShopperProCreditSourceType[inputType];
            
                return updatedType;
            } }
        ],
        "searching": false,
        destroy: true
    });

    dataTable.on('draw', function () {
        $("#creditsTable tbody .edit-button").off().click(function (event) {
            ClearFormValidation();
            var creditCode = $(this).closest("tr").find(">:first-child").text();
        });
    });
}

function FindSelectedValueInTableForRenewing()
{
    var selectedProCreditIds = [];

    var checkboxes = document.querySelectorAll("#creditsTable input[type='checkbox']");
  
    for (var i = 0; i < checkboxes.length; i++) {
      if (checkboxes[i].checked) {
        var row = checkboxes[i].closest("tr");

        const timeElapsed = Date.now();
        const today = new Date(timeElapsed);

        if ((new Date(row.querySelectorAll("td")[3].textContent).getTime() < today.getTime()) && 
        !row.querySelectorAll("td")[2].textContent) {
            var proCreditId = row.querySelectorAll("td")[1].textContent;

            selectedProCreditIds.push(proCreditId);
        } else {
            alert("Only expired and not redeemed pro credits can be renewed.");
            return selectedProCreditIds;
        }
      }
    }
      return selectedProCreditIds;
}

function FindSelectedValueInTableForDeleting()
{
    var selectedProCreditIds = [];

    var checkboxes = document.querySelectorAll("#creditsTable input[type='checkbox']");
  
    for (var i = 0; i < checkboxes.length; i++) {
      if (checkboxes[i].checked) {
  
        var row = checkboxes[i].closest("tr");

        const timeElapsed = Date.now();
        const today = new Date(timeElapsed);
  
        if (!row.querySelectorAll("td")[2].textContent) {
            var proCreditId = row.querySelectorAll("td")[1].textContent;

            selectedProCreditIds.push(proCreditId);
        } else {
            alert("Redeemed pro credits cannot be deleted.");
            return selectedProCreditIds;
        }
      }
    }
      return selectedProCreditIds;
}

$("#addNewProCreditsButton").click(function (event) {
    event.preventDefault();
    event.stopPropagation();
    AddProCredits();
});

function AddProCredits() {
    var shopperId = document.querySelector("#shopperIdField").value;
    var proCreditsQuantity = document.querySelector("#proCreditsQuantityField").value;
    var creditValue = document.querySelector("#creditValueField").value;

    if (ValidateForm('#createProCreditsModal form :input', "#createModalError") === false) {
        return;
    }

    $("#createModalError").hide();

    $.ajax({
        url: window.webServiceURL + "/api/pro-credit/add",
        type: "POST",
        contentType: 'application/json',

        data: JSON.stringify(
            { 
                "quantityToBeAdded": proCreditsQuantity ,
                "shopperId": shopperId,
                "creditValue": creditValue
            }),
    })
    .done(function (data) {
        $('#createProCreditsModal').modal("hide");

        GetTableData(shopperId);
    })
        .fail(function (xhr, status, text) {
            parsedError = JSON.parse(xhr.responseText);
            $("#createModalError").empty().html("<b>Error</b><br/>" + parsedError.Message).show();
    })
    .always(function () {
        $("form :input, button").prop("disabled", false);
        alwaysCallback();
    });
}

function RenewProCredits() {
    var shopperId = document.querySelector("#inputShopperId").value;
    var selectedProCreditIds = FindSelectedValueInTableForRenewing();

    if (selectedProCreditIds.length > 0)
    {
        $.ajax({
            url: window.webServiceURL + "/api/pro-credit/renew-pro-credit",
            type: "POST",
            contentType: 'application/json',
    
            data: JSON.stringify(
                { 
                    "shopperProCreditsIds": selectedProCreditIds
                }),
        })
        .done(function (data) {
            GetTableData(shopperId);
        }); 
    }
}

function RemoveProCredits() {
    var shopperId = document.querySelector("#inputShopperId").value;
    var selectedProCreditIds = FindSelectedValueInTableForDeleting();

    if (selectedProCreditIds.length > 0)
    {
        $.ajax({
            url: window.webServiceURL + "/api/pro-credit/remove",
            type: "DELETE",

            contentType: 'application/json',

            data: JSON.stringify(
                { 
                    "shopperProCreditsIds": selectedProCreditIds
                }),
        })
        .done(function (data) {
            GetTableData(shopperId);
        });
    }
}

function Autocomplete(selector, sourceURL) {

    var searchBox = $(selector);
    var autocompleteItems = searchBox.siblings(".autocomplete-items");
    autocompleteItems.mouseout(function () {
        searchBox.focus();
    });

    searchBox
        .keydown(function (e) {
            clearTimeout(window.autocompleteTimerID);

            if (e.keyCode === 27) {
                autocompleteItems.hide();
                return;
            }
            var q = searchBox.val().trim();

            if (!q) {
                autocompleteItems.hide();
            }
            else {
                window.autocompleteTimerID = setTimeout(function () {
                    // Search
                    if (window.inAutoCompleteSeach === true) return;

                    var q2 = searchBox.val().trim();
                    if (!q2) {
                        autocompleteItems.hide();
                        window.inAutoCompleteSeach = false;
                        return;
                    }

                    window.inAutoCompleteSeach = true;
                    autocompleteItems.empty();
                    searchBox.addClass("autocomplete-loading");
                    $.get(sourceURL + q2, function (data) {
                        if (data.length > 0) {
                            $.each(data, function (index, value) {
                                var elem = $("<a href='#'>" + value + "</a>");
                                autocompleteItems.append(elem);
                                elem
                                    .on("click", function () {
                                        event.preventDefault();
                                        event.stopPropagation();
                                        searchBox.val($(this).text());
                                        autocompleteItems.hide();
                                    })
                                    .on("mouseenter", function () {
                                        window.inAutoCompleteLink = true;
                                    })
                                    .on("mouseleave", function () {
                                        window.inAutoCompleteLink = false;
                                    });
                            });
                            autocompleteItems.show();
                        }
                        else {
                            autocompleteItems.hide();
                        }
                        window.inAutoCompleteSeach = false;
                        searchBox.removeClass("autocomplete-loading");
                    });
                }, 600);
            }
        })
        .focusout(function () {
            searchBox.removeClass("autocomplete-loading");
            if (window.inAutoCompleteLink !== true) {
                autocompleteItems.hide();
            }
        });
}

function ValidateForm(inputSelector, errorContainerSelector)
{
    var isValid = true;
    ClearFormValidation();
    var list = $(errorContainerSelector).empty().append("<ul></ul>").show();
    var $inputs = $(inputSelector);
    $inputs.each(function () {
        var val = $(this).val();
        var labelText = this.name;
        var label = $(this).siblings("label");
        if (label.length > 0) labelText = label.text();
        if ($(this).data("required") == true && (typeof(val) === "undefined" || !val)) {
            list.append("<li><b>" + labelText + "</b> is required</li>")
            isValid = false;
            $(this).closest(".form-group").addClass("has-error");
        }
    });
    return isValid;
}

function ClearFormValidation() {
    $("#createModalError").empty().hide();
    $("#createProCreditsModal .form-group").removeClass("has-error");
}

function GetWebServiceURL() {
    var host = window.location.host;
	return "https://" + host + ":27019";
}

function ShowEnvironmentLabel() {
    var host = window.location.host;
    if (host == "dev-csc.musicnotes.com") {
        $("#environmentLabel").attr("class", "label label-warning").text("dev");
    }
    else if (host == "csc.musicnotes.com") {
        $("#environmentLabel").attr("class", "label label-primary").text("prod");
    }
    else {
        $("#environmentLabel").attr("class", "label label-default").text("local");
    }
}