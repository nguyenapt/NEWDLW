module Netafim.Web {

    interface IInput {
        actionUrl: string
    }

    export class SystemConfiguratorParametersBlock {

        private parameter: IInput;

        constructor(jqueryElement: any, parameter: IInput) {

            this.parameter = parameter;
            var self = this;

            var current_step = 1;
            
            checkStepOnMobile();
            function checkStepOnMobile() {
                switch (current_step) {
                    case 1:
                        $('.show-this-step',jqueryElement).removeClass('show-this-step');
                        $('.config-form-progress .active-step', jqueryElement).removeClass('active-step');
                        $('.config-form-progress .form-step-1', jqueryElement).addClass('active-step');
                        $('.config-form-step', jqueryElement).first().addClass('show-this-step');
                        $('.config-form-step-navigation-button', jqueryElement).removeClass('last-step');
                        $('.config-form-step-navigation-button', jqueryElement).addClass('first-step');
                        break;
                    case 2:
                        $('.show-this-step', jqueryElement).removeClass('show-this-step');
                        $('.config-form-progress .active-step', jqueryElement).removeClass('active-step');
                        $('.config-form-progress .form-step-1', jqueryElement).addClass('active-step');
                        $('.config-form-progress .form-step-2', jqueryElement).addClass('active-step');
                        $('.config-form-step', jqueryElement).first().next().addClass('show-this-step');
                        $('.config-form-step-navigation-button', jqueryElement).removeClass('last-step');
                        $('.config-form-step-navigation-button', jqueryElement).removeClass('first-step');
                        break;
                    case 3:
                        $('.show-this-step', jqueryElement).removeClass('show-this-step');
                        $('.config-form-progress .form-step-1', jqueryElement).addClass('active-step');
                        $('.config-form-progress .form-step-2', jqueryElement).addClass('active-step');
                        $('.config-form-progress .form-step-3', jqueryElement).addClass('active-step');
                        $('.config-form-step', jqueryElement).last().addClass('show-this-step');
                        $('.config-form-step-navigation-button', jqueryElement).addClass('last-step');
                        $('.config-form-step-navigation-button', jqueryElement).removeClass('first-step');
                        break;
                }
            }

            function validateStepConfig(step) {
                var _valid = false;
                if (step != "all") {
                    $('.config-form-step.show-this-step', jqueryElement).find('select').each(function () {
                        if ($(this).val() == null) {
                            $(this).parent().next('.error-message').show();
                            _valid = false;
                        } else {
                            $(this).parent().next('.error-message').hide();
                            _valid = true;
                        }
                    });
                    $('.config-form-step.show-this-step', jqueryElement).find('input[type="number"]').each(function () {
                        if (!$(this).val()) {
                            $(this).parent().next('.error-message').show();
                            _valid = false;
                        } else {
                            $(this).parent().next('.error-message').hide();
                            _valid = true;
                        }
                    });
                } else {
                    $('.config-form-wrapper', jqueryElement).find('select').each(function () {
                        if ($(this).val() == null) {
                            $(this).parent().next('.error-message').show();
                            _valid = false;
                        } else {
                            $(this).parent().next('.error-message').hide();
                            _valid = true;
                        }
                    });
                    $('.config-form-wrapper', jqueryElement).find('input[type="number"]').each(function () {
                        if (!$(this).val()) {
                            $(this).parent().next('.error-message').show();
                            _valid = false;
                        } else {
                            $(this).parent().next('.error-message').hide();
                            _valid = true;
                        }
                    });
                }
                if (_valid) {
                    return true;
                } else {
                    return false;
                }
            }
            //Custom number input
            $('.config-form-wrapper .FormNumber', jqueryElement).each(function () {
                var input = $(this).find('input[type="number"]').first();
                var btnUp = $(this).find('.number-up');
                var btnDown = $(this).find('.number-down');
                var min = 0;
                if (input.attr('min')) {
                    min = parseInt(input.attr('min'));
                } else {
                    min = 0;
                }


                var max = 0;
                if (input.attr('max')) {
                    max = parseInt(input.attr('max'));
                } else {
                    var max = 1000;
                }
                btnUp.on('click', function () {

                    if (!input.val()) {
                        input.val(0);
                    }
                    var oldValue = parseFloat(input.val());
                    if (oldValue >= max) {
                        var newVal = oldValue;
                    } else {
                        var newVal = oldValue + 1;
                    }
                    input.val(newVal);
                    input.trigger("change");
                });
                btnDown.on('click', function () {
                    if (!input.val()) {
                        input.val(0);
                    }
                    var oldValue = parseFloat(input.val());
                    if (oldValue <= min) {
                        var newVal = oldValue;
                    } else {
                        var newVal = oldValue - 1;
                    }
                    input.val(newVal);
                    input.trigger("change");
                });
            });
            $('.config-form-step-navigation-button .btn-next-step', jqueryElement).on('click', function () {
                if (validateStepConfig("abc")) {
                    current_step = current_step + 1;
                }
                checkStepOnMobile();
            });
            $('.config-form-step-navigation-button .btn-previous-step', jqueryElement).on('click', function () {
                current_step = current_step - 1;
                checkStepOnMobile();
            });
            $('.system-configurator').on('submit', function (e) {
                if (!validateStepConfig("all")) {
                    e.preventDefault();
                }
            });

            //$('.config-form-step-navigation-button .btn-generate', jqueryElement).on('click', function () {
            //    $.ajax({
            //        url: self.parameter.actionUrl,
            //        data: {},
            //        contentType: 'json',
            //        method: 'post',
            //        success: function (res) {
            //            var d = res;
            //        },
            //        error: function (err) {

            //        }
            //    });
            //})
        }
    }
}