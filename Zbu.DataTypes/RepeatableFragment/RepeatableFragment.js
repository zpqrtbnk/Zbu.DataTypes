// registers the zbu plugin
(function ($) {
    if (!$.fn.zbu) {
        var methods = {};
        $.fn.zbu = function (method) {
            // note: 'this' is already a jQuery object, here
            if (methods[method]) {

                // capture, since we're nesting methods call, below
                var args = arguments;

                // will detect when a value is returned, stop each() by returning false,
                // and return the value. so, methods can return values. but those values can NOT
                // be null/nothing/undefined...
                var value;
                var each = this.each(function () {
                    if (value = methods[method].apply(this, Array.prototype.slice.call(args, 1)))
                        return false;
                    else
                        return true;
                });
                return value ? value : each;

            } else if (typeof method === 'object' || !method) {
                // there is no 'init' in our case
                //return methods.init.apply(this, arguments);
                return this;
            } else {
                $.error('Method ' + method + ' does not exist on jQuery.zbu');
                return this;
            }
        };
        $.zbudef = function(name, method) {
            methods[name] = method;
        };
    }
})(jQuery);

// register the repeatable fragment method
(function($) {
    $.zbudef("repeatable-fragment", function (args) {
        var $this = $(this);
        var thisId = $this.attr('id');
        thisId = thisId.substr(0, thisId.length - '_edit'.length);

        var addButton = $(this).find('a');
        addButton.css('color', 'green');

        function updateFragment(value) {
            if (value.outVal)
                $('#' + thisId + '_data').val(value.outVal);
        }

        //var value = $('#' + thisId + '_data').val();
        //if (value)
        //    value = value.trim();
        //if (!value)
        //    value = '{ "FragmentTypeAlias": "' + args.fragmentTypeAlias + '" }';
        //$('#' + thisId + '_data').val(value);

        $('#' + thisId + '_add').click(function () {
            // url, title, showHeader, width, height, top, left, closeTriggers, onCloseCallback
            UmbClientMgr.openModalWindow(args.umbraco + '/plugins/Zbu/RepeatableFragment/EditFragment.aspx'
                + '?id=' + args.contentId
                //+ '&ctype=' + args.fragmentTypeAlias
                + '&json=' + encodeURIComponent($('#' + thisId + '_data').val()),
                'Edit fragment', true, 800, 680, 10, 0, '', updateFragment);
        });
    });
})(jQuery);
