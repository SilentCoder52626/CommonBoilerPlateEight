function SuccessToast(message) {
    Swal.fire({
        icon: 'success',
        html: message,
        toast: true,
        iconColor: 'green',
        position: 'top-right',
        showConfirmButton: false,
        timer: 3000,
        customClass: {
            popup: 'class-white'
        },
    });
}

function FailureToast(message) {
    Swal.fire({
        icon: 'error',
        html: message,
        toast: true,
        iconColor: 'red',
        position: 'top-right',
        showConfirmButton: false,
        timer: 3000,
        customClass: {
            popup: 'text-white'
        },
    });
}

function InfoToast(message) {
    Swal.fire({
        icon: 'info',
        html: message,
        toast: true,
        position: 'top-right',
        showConfirmButton: false,
        timer: 3000,
        customClass: {
            popup: 'text-white'
        },
    });
}



function ShowDialog(title, text, status) {
    return Swal.fire({
        title: title,
        text: text,
        icon: status,
        showCancelButton: true,
        confirmButtonText: 'Yes',
        cancelButtonText: 'No',
    });
}
function ShowDialogWithTextInput(title, text, status) {
    return Swal.fire({
        title: title,
        text: text,
        icon: status,
        input: 'textarea',
        inputPlaceholder: 'Type your message here...',
        inputAttributes: {
            'aria-label': 'Type your message here'
        },
        showCancelButton: true,
        confirmButtonText: 'Submit',
        cancelButtonText: 'Cancel',
    });
}


function blockwindow(message = "Please wait ....") {
    $.blockUI({
        message:
            `<div class="d-flex justify-content-center"><p class="mb-0">${message}.</p> <div class="sk-wave m-0"><div class="sk-rect sk-wave-rect"></div> <div class="sk-rect sk-wave-rect"></div> <div class="sk-rect sk-wave-rect"></div> <div class="sk-rect sk-wave-rect"></div> <div class="sk-rect sk-wave-rect"></div></div> </div>`,
        css: {
            backgroundColor: 'transparent',
            color: '#fff',
            border: '0'
        },
        overlayCSS: {
            opacity: 0.5
        }
    });
}

function unblockwindow() {
    $.unblockUI();
}

function DisabledAllInputs($element = null) {
    debugger;
    if (!$element) {
        $element = $(document);
        disableInputs = true; // set to true only if called for whole document
    } else {
        $element = $($element); // fail safe
    }
    $element.find("input").attr("readonly", "readonly");

    $element.find("button").css('display', 'none');
    $element.find(".hide-detail").remove();
    $element.find("input[type='button']").css('display', 'none');
    $element.find("input[type='file']").css('display', 'none');
    $element.find("select").attr("disabled", "disabled");
    $element.find("form").find("input[type='submit']").remove();
    $element.find("form").find("input[type='button']").css('display', 'none');
    $element.find("form").find(':radio:not(:checked)').attr('disabled', true);
    $element.find("form").find("select").attr("disabled", "disabled");
    $element.find("form").find("a[id='resetpassword']").remove();
    $element.find("form").find("button").css('display', 'none');
    $element.find("form").find("input[type='checkbox']").attr('disabled', true);
    $element.find("form").find("textarea").attr('disabled', true);
    $element.find(".ClassToHide").css('display', 'none');
    $element.find('input[type=text], textarea').attr('disabled', true);
    $element.find('input[type=hidden]').attr('disabled', false);

}

function exportAllFilterData(e, dt, button, config) {
    var self = this;
    var oldStart = dt.settings()[0]._iDisplayStart;
    dt.one('preXhr', function (e, s, data) {
        // Just this once, load all data from the server...
        data.start = 0;
        data.length = dt.page.info().recordsTotal;
        dt.one('preDraw', function (e, settings) {
            // Call the original action function
            if (button[0].className.indexOf('buttons-copy') >= 0) {
                $.fn.dataTable.ext.buttons.copyHtml5.action.call(self, e, dt, button, config);
            } else if (button[0].className.indexOf('buttons-excel') >= 0) {
                $.fn.dataTable.ext.buttons.excelHtml5.available(dt, config) ?
                    $.fn.dataTable.ext.buttons.excelHtml5.action.call(self, e, dt, button, config) :
                    $.fn.dataTable.ext.buttons.excelFlash.action.call(self, e, dt, button, config);
            } else if (button[0].className.indexOf('buttons-csv') >= 0) {
                $.fn.dataTable.ext.buttons.csvHtml5.available(dt, config) ?
                    $.fn.dataTable.ext.buttons.csvHtml5.action.call(self, e, dt, button, config) :
                    $.fn.dataTable.ext.buttons.csvFlash.action.call(self, e, dt, button, config);
            } else if (button[0].className.indexOf('buttons-pdf') >= 0) {
                $.fn.dataTable.ext.buttons.pdfHtml5.available(dt, config) ?
                    $.fn.dataTable.ext.buttons.pdfHtml5.action.call(self, e, dt, button, config) :
                    $.fn.dataTable.ext.buttons.pdfFlash.action.call(self, e, dt, button, config);
            } else if (button[0].className.indexOf('buttons-print') >= 0) {
                $.fn.dataTable.ext.buttons.print.action(e, dt, button, config);
            }
            dt.one('preXhr', function (e, s, data) {
                // DataTables thinks the first item displayed is index 0, but we're not drawing that.
                // Set the property to what it was before exporting.
                settings._iDisplayStart = oldStart;
                data.start = oldStart;
            });
            // Reload the grid with the original page. Otherwise, API functions like table.cell(this) don't work properly.
            setTimeout(dt.ajax.reload, 0);
            // Prevent rendering of the full data to the DOM
            return false;
        });
    });
    // Requery the server with the new one-time export settings
    dt.ajax.reload();
};


$('.select2Input').select2({ placeholder: "Select", allowClear: true, width: '100%' });


function handleAjaxError(response) {
    if (response.status == 403) {
        FailureToast("Unauthorized access");
    } else if (response.responseJSON && response.responseJSON.Status == "Info") {
        InfoToast(response.responseJSON.Errors.join("<br>"));
    } else {
        FailureToast("Something went wrong. Please contact the administrator.");
    }

}


$('.select-with-flags').select2({
    templateResult: formatCountryOption,
    templateSelection: formatCountryOption,
    matcher: matchCustom
});
function formatCountryOption(option) {
    if (!option.id) {
        return option.text;
    }
    var flagCode = $(option.element).data('flag');
    var dialCode = $(option.element).data('dial-code');
    var countryName = $(option.element).data('country');
    var flagIcon = `<span class='flag-icon flag-icon-${flagCode}' style='margin-right: 8px;'></span>`;
    var formattedText = `${flagIcon} <span>${dialCode} </span>`;
    return $(formattedText);
}
function matchCustom(params, data) {

    if ($.trim(params.term) === '') {
        return data;
    }
    if (data.text.toLowerCase().indexOf(params.term.toLowerCase()) > -1 ||
        $(data.element).data('country').toLowerCase().indexOf(params.term.toLowerCase()) > -1) {
        return data;
    }


    return null;
}


function initializePicker(selector, options = {}) {
    const defaultOptions = {
        allowInput: false,
        enableTime: false,  // Default is date picker only
        noCalendar: false,
        time_24hr: false,
        dateFormat: "Y-m-d",  // Default date format
        minDate: null,        // No restriction by default
        maxDate: null         // No restriction by default
    };

    const config = Object.assign(defaultOptions, options);

    const elements = document.querySelectorAll(selector);
    elements.forEach(element => {
        // Check for data-show attribute to determine date restriction
        if (element.hasAttribute('data-show')) {
            debugger;
            const showType = element.getAttribute('data-show');
            switch (showType) {
                case 'past':
                    config.maxDate = "today";  // Restrict to past and today
                    break;
                case 'future':
                    config.minDate = "today";  // Restrict to future and today
                    break;
                case 'all':
                default:
                    config.minDate = null;     // Allow all dates
                    config.maxDate = null;
                    break;
            }
        }

        // Determine pickerType via data attributes (time, date, datetime)
        if (element.hasAttribute('data-type')) {
            
            const pickerType = element.getAttribute('data-type');
            switch (pickerType) {
              
                case 'time':
                    config.enableTime = true;
                    config.noCalendar = true;
                    config.dateFormat = "h:i K";  // Time format
                    break;
                case 'datetime':
                    config.enableTime = true;
                    config.noCalendar = false;
                    config.dateFormat = "Y-m-d h:i K";  // Date and time format
                    break;
                case 'date':
                default:
                    config.enableTime = false;
                    config.noCalendar = false;
                    config.dateFormat = "Y-m-d";  // Date format
                    break;
            }
        }

 

        // Initialize flatpickr for the element
        flatpickr(element, config);
    });
}

initializePicker('.datetime-picker');




function previewImage(event) {
    const file = event.target.files[0];
    const allowedTypes = ["image/jpeg", "image/png"];
    const imageContainer = event.target.getAttribute('data-image-container');
    const imageElm = document.getElementById(imageContainer);
    if (file && allowedTypes.includes(file.type)) {
        const reader = new FileReader();
        reader.onload = function (e) {
            imageElm.src = e.target.result;
        };
        reader.readAsDataURL(file);
    } else {

        alert("Only JPG and PNG images are allowed.");
        event.target.value = "";
        imageElm.src = "/images/others/placeholder.jpg";
    }
}
