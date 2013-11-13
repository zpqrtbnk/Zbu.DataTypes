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

        var count = 0;
        var editing = -1;

        function updateFragment(value) {
            if (value.outVal && editing >= 0)
                $('#' + thisId + '_' + editing).val(value.outVal);
            editing = -1;
        }
        
        function addFragment(data) {
            var index = count++;

            // add the index to the list
            var idx = $('#' + thisId + '_idx');
            idx.val(idx.val() + index + ',');            

            var fragmentHtml = '<div class="zbu-fragment">'
                + '<input type="hidden" id="' + thisId + '_' + index + '" name="' + thisId + '_' + index + '" value="" />'
                + '<a href="#" class="zbu-fragment-remove">remove</a>'
                + '<a href="#" class="zbu-fragment-edit">edit</a>'
                + '<div class="zbu-fragment-head">Blah</div>' // FIXME title management
                + '</div>';
            
            // create the fragment and set its data value
            var fragment = $(fragmentHtml);
            fragment.appendTo($this.find('.zbu-fragments'));
            fragment.disableSelection();
            $('#' + thisId + '_' + index).val(data);

            // plug the edit button
            fragment.find('a.zbu-fragment-edit').click(function(event) {
                editing = index;
                // url, title, showHeader, width, height, top, left, closeTriggers, onCloseCallback
                UmbClientMgr.openModalWindow(args.umbraco + '/plugins/Zbu/RepeatableFragment/EditFragment.aspx'
                    + '?id=' + args.contentId
                    + '&fragment=' + encodeURIComponent($('#' + thisId + '_' + index).val()),
                    'Edit fragment', true, 800, 680, 10, 0, '', updateFragment);
                event.preventDefault();
            });

            // plug the remove button
            fragment.find('a.zbu-fragment-remove').click(function (event) {

                var idx = $('#' + thisId + '_idx');
                idx.val(idx.val().replace('' + index + ',', ''));

                fragment.remove();
                event.preventDefault();
            });
        }

        // create initial fragments
        for (var i = 0; i < args.fragments.length; i++)
            addFragment(JSON.stringify(args.fragments[i]));
        
        // plug add button
        $this.find('.zbu-fragments-add').click(function () {
            addFragment(args.fragment);
        });

        // enable sorting of fragments
        $this.find('.zbu-fragments').sortable({
            items: '.zbu-fragment',
            placeholder: 'zbu-sortable-placeholder',
            forcePlaceholderSize: true,
            update: function (event, ui) {
                // update the list when order changes
                var idx = ',';
                var len = (thisId + '_').length;
                $this.find('.zbu-fragment input:hidden').each(function() {
                    idx += $(this).attr('id').substr(len) + ',';
                });
                $('#' + thisId + '_idx').val(idx);
            }
        });
    });
})(jQuery);
