var $BaseUrl = "../";
$(document).ready(function () {
    $('.navbar-toggle').click(function () {
        $('.navbar-nav').toggleClass('slide-in');
        $('.side-body').toggleClass('body-slide-in');
        $('#search').removeClass('in').addClass('collapse').slideUp(200);

        /// uncomment code for absolute positioning tweek see top comment in css
        //$('.absolute-wrapper').toggleClass('slide-in');

    });
});
function OpenCollapseSubMenu() {
    $('#sub-menu').toggleClass('in');
}
$(document).ready(function () {
    var lanName = $('body').attr('data-lan');
    $('#lst_language').val(lanName);
})
$(document).mouseup(function (e) {
    var container1 = $("#CaptchaText");
    var container2 = $(".form-control");

    if (!container1.is(e.target) && container1.has(e.target).length === 0 || !container2.is(e.target) && container2.has(e.target).length === 0) {
        $(container1).removeAttr('placeholder');
        $(container2).removeClass('alpha_validate');

    }

});
$(document).on('click', '.menu-item', function () {
  
    var $now = moment(new Date($.now())).subtract(1, "days");
    var thisYear = (new Date()).getFullYear();
    var start = new Date("1/1/" + thisYear);
    var defaultStart = moment(start.valueOf()).format('DD-MM-YYYY');;
    var $currentYear = moment().format('YYYY');
    var $url = $(this).data('url');
    EnableLoader();
    $.ajax({
        type: "GET",
        url: $BaseUrl + $url,
        success: function (response) {
            $('#page_container').empty();
             DisableLoader();
            $('#page_container').html(response);
            setTimeout(function () {
                $(".bts_select").select2();
                $('.alpha-dp_body').datetimepicker({
                    format: 'DD/MM/YYYY',
                });
                $('input[id="FromDate"]').each(function () {
                    $(this).val(defaultStart);
                })
                $('input[id="ToDate"]').each(function () {
                    $(this).val($now.format('DD-MM-YYYY'));
                })
                $('#Year').val($currentYear);
                $('#fromMonth').val(1);
                $('#toMonth').val(12);
                $('.alpha-year').datetimepicker({
                    format: 'YYYY',
                });
                $('input[name="FromStore"]').trigger('change');
                $('input[name="ToStore"]').trigger('change');
                $('.ExpiryDateComparison').find('#FromDate').val(moment('1-1-2000').format('DD-MM-YYYY'));
                $('.ExpiryDateComparison').find('#ToDate').val(moment('1-1-2070').format('DD-MM-YYYY'));
                $('input[name="FromCountry"]').val(1).trigger('change');
            })
        },
        error: function () {
            DisableLoader();
        }
    });

})

$(document).on('change', '.mounth', function (e) {
    var $elem = e.target;
    var $val =$($elem).val();
    if ($val > 12 || $val <= 0)
        $($elem).val(1);
})

function FilterMenu($txt) {
    var $val = $(this).val().toLowerCase();
    $('.side-menu').find('.panel-collapse').removeClass('in');
    $('.side-menu').find('.panel-collapse').not('div#search').addClass('collapse');
    $('.side-menu .navbar-nav li').removeClass('active_menu');
    $('.side-menu').find('li').show();
    if ($val === '')
        return false;
    $('.side-menu .navbar-nav').find('ul > li').not('#dropdown').each(function () {
        var $lis = $(this).find('a').text().toLowerCase();
        if ($lis.indexOf($val) != -1) {
            var $level = $(this).parents('.panel-collapse');
            $($level).addClass('in');
            $(this).addClass("active_menu")
            $(this).find('a').css('color', 'white');
            $(this).parents('.nav.navbar-nav:first').css('background-color', '#00173d')
        }
        else { $(this).hide(); }
    });
}
function EnableLoader() {
    $('.loader').removeClass('hidden');
    $(document).css('pointer-events', 'none');
}
function DisableLoader() {
    $('.loader').addClass('hidden');
    $(document).css('pointer-events', 'unset');
}
function RefreshCaptch(elem) {
    $.ajax({
        type: "POST",
        url: $BaseUrl + 'Account/CaptchaPartail',
        async :false,
        success: function (response) {
            $('#Captcha').find('img').remove();
            $('#Captcha').html(response);
        },

    });

}

function ChangeLangauge() {
    $.ajax({
        type: "POST",
        url: '../../Account/ChangeLanguage',
        success: function () {
            location.reload();
        },
    });
}
function SetLangauge($lang) {
    $.ajax({
        type: "POST",
        url: $BaseUrl + 'Account/ChangeLanguage',
        data: { lanuage: $lang },
        success: function (response) {
            location.reload();
        },
    });
}
function ReportValidation(form) {
    var $momentFirstDate = moment('1/1/1753').format('DD/MM/YYYY');
    var $momentEndDate = moment('1/1/9999').format('DD/MM/YYYY');
    /*****Check Empty******/
    if (Check_RQ_Field(form) == false)
        return false;
    /*********Check Date*********/
    for (var i = 0; i < $(form).find('.alpha-date-container').length; i++) {
        var $alphaDateContainer = $(form).find('.alpha-date-container')[i];
        var $fromDate = $($alphaDateContainer).find('#FromDate').val();
        var $toDate = $($alphaDateContainer).find('#ToDate').val();
        if (Check_EndDate_GreaterThan_FormDate($fromDate, $toDate, form) == false) {
            $('#Date_Modal').modal('show');
            return false;
        }

    }
    for (var i = 0; i < $(form).find('input[id="FromDate"]').length; i++) {
        var $fromDateInput = $(form).find('input[id="FromDate"]')[i];
        var $fromDate = $($fromDateInput).val().split('/');
        if ($fromDate == undefined )
            return true;
        if (+$fromDate[2] > 2079 || +$fromDate[2] < 1900) {
            $('#WrongDatePeriod').modal('show');
            return false;
        }
    }
    for (var i = 0; i < $(form).find('input[id="ToDate"]').length; i++) {
        var $toDateInput = $(form).find('input[id="ToDate"]')[i];
        var $toDate = $($toDateInput).val().split('/');
        if ( $toDate == undefined)
            return true;
        if (+$toDate[2] > 2079 || +$toDate[2] < 1900) {
            $('#WrongDatePeriod').modal('show');
            return false;
        }
    }
    /*******Check Year*******/
    for (var i = 0; i < $(form).find('input[id="Year"]').length; i++) {
        var $year = $(form).find('input[id="Year"]')[i];
        var $yearVal = $($year).val();
        if ($yearVal == undefined)
            return true;
        if (+$yearVal > 2079 || +$yearVal < 1900) {
            $('#WrongDatePeriod').modal('show');
            return false;
        }
    }

}
function Check_RQ_Field(container) {
    var $flag = true;
    for (var i = 0; i < $(container).find('input , select').length; i++) {
        var $input = $(container).find('input , select')[i];
        var $tagName = $($input).prop("tagName");
        if ($tagName === 'INPUT' && $($input).val().trim() === '') {
            $($input).addClass('alpha_validate');
            if ($($input).hasClass('alpha-erp-portal-date')) {
                return false;
            }
            else {
                $($input).closest('.inner-addon').find('i.glyphicon').addClass('glyphicon-exclamation-sign');
                return false;
            }
        }
        else if ($tagName === 'SELECT' && $($input).val() === 0) {
            var $container = $(this).parents('.select2-content')[0]
            $($container).find('.select2-selection').addClass('alpha_validate');
            return false;

        }
    }
    return true;

}
function Check_EndDate_GreaterThan_FormDate(FormDate, ToDate , form) {
    var StartDate = moment(FormDate, 'DD/MM/YYYY');
    var EndDate = moment(ToDate, 'DD/MM/YYYY');
    if (moment(EndDate).isSameOrAfter(StartDate) == false) {
        return false;
    }
    return true;
}
function GetStroreByName(elem) {
    var $container = $(elem).parents('div.store')[0];
    $($container).find('.alpha-portal-number').val($(elem).val());
}
function GetStroreByID(elem) {
    var $container = $(elem).parents('div.store')[0];
    var $val = $(elem).val();
    $($container).find('select').val($val).trigger('change');
   
}

/****/
function GetCategoryByName(elem) {
    var $container = $(elem).parents('div.category')[0];
    $($container).find('.alpha-portal-number').val($(elem).val());
}
function GetCategoryByID(elem) {
    var $container = $(elem).parents('div.category')[0];
    var $val = $(elem).val();
    if ($val.startsWith("9999") || $val.toLowerCase().startsWith('zzzzz') || $val == 0) {
        $($container).find('.alpha-portal-number').removeClass('alpha-portal-number');
        $($container).find('select').val(0).trigger('change');
        $($container).find('input[type=text]').addClass('alpha-portal-number');
        return false;
    }
    else
        $($container).find('select').val($val).trigger('change');
}

/*****************/
function GetCountryByName(elem) {
    var $container = $(elem).parents('div.Country')[0];
    $($container).find('.alpha-portal-number').val($(elem).val());
}
function GetCountryByID(elem) {
    var $container = $(elem).parents('div.Country')[0];
    var $val = $(elem).val();
   $($container).find('select').val($val).trigger('change');
}
