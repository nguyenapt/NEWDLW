define([
    "dojo/_base/array",
    "dojo/_base/connect",
    "dojo/_base/declare",
    "dojo/_base/lang",

    "dijit/_CssStateMixin",
    "dijit/_Widget",
    "dijit/_TemplatedMixin",
    "dijit/_WidgetsInTemplateMixin",
    "dijit/dijit", // loads the optimized dijit layer
    "epi/shell/widget/DateTimeSelectorDropDown",
    "epi/epi"

],
function (
    array,
    connect,
    declare,
    lang,
    _CssStateMixin,
    _Widget,
    _TemplatedMixin,
    _WidgetsInTemplateMixin,
    dijit,
    DateTextBox,
    epi
) {

    return declare("netafim.UTCDateTimeTextBox", [_Widget, _TemplatedMixin, _WidgetsInTemplateMixin, _CssStateMixin], {
        templateString: "<div class=\"dijitInline\"> \
                            <input data-dojo-attach-point=\"dateTextBox\" data-dojo-type=\"epi/shell/widget/DateTimeSelectorDropDown\" required=\"true\"/> \
                        </div>",
        constructor: function () {
        },

        _onLoad: function () {
            this.dateTextBox.constraints = {
                formatLength: "short",
                fullYear: "true"
            };

            this.inherited(arguments);
            var dd = new Date(this.value);
            if (dd != undefined) {
                if (!isNaN(dd.getTime())) {
                    //Convert to local time
                    dd = new Date(dd.getTime() + (dd.getTimezoneOffset() * 60000));
                }
            }
            var _this = this;
            this.dateTextBox.set('value', dd);
            dojo.connect(this.dateTextBox, "onChange", function (value) {
                _this._setValue(value);
            });
        },
        // Setter for value property
        _setValueAttr: function (value) {
            this._setValue(value, true);
            this._onLoad();
        },

        onChange: function (value) {
            console.log(value);
            // Event that tells EPiServer when the widget's value has changed.
        },

        _getValueAttr: function () {
            var date = this.dateTextBox.get('value');
            if (date == undefined)
                return date;

            if (isNaN(date.getTime()))
                return date;
            //Convert to UTC time to make it locale independent
            date = new Date(date.getTime() - (date.getTimezoneOffset() * 60000));
            return date;
        },

        _setReadOnlyAttr: function (value) {
            this._set("readOnly", value);
        },

        _setValue: function (value) {
            if (this._started && epi.areEqual(this.value, value)) {
                return;
            }

            // set value to this widget (and notify observers)
            this._set("value", value);
        }
    });
});