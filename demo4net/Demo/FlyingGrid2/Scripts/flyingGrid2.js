/*---------------------------
 名称：flyingGrid2
 作者：weichengdong
 邮件: weichengdong2008@foxmail.com
 ---------------------------*/
(function ($) {
    $.DefaultCfg = {
        cache: false,
        type: "POST",
        url: "#",
        data: {}
    };

    $.Pager = {
        pageNumber: 1
    };

    $.Cfg = {};
    $.Grid = {};
    $.GridContainer = {};
    /*---------------------------
    加载表格  $.fn.LoadGrid
    ---------------------------*/
    $.fn.LoadGrid = function () {
        var pager = $.extend({}, $.Pager);
        $.AjaxGetInfo(pager);
    };

    /*---------------------------
    初始化  $.fn.flyingGrid2
    ---------------------------*/
    $.fn.flyingGrid2 = function (options) {
        $.Cfg = $.extend({}, $.DefaultCfg, options);
        $.GridContainer = this;
        $.Grid = this.find("#" + this.attr("grid"));
        $.UsePage(); //如果不是ajax方式而是把表格直接绑定页面时显示表尾部控制栏
        return this.each(function () {
            $.InitEvent();
        });
    };

    /*---------------------------
    小图标鼠标动画效果  $.MouseoveE $.MouseoutE
    ---------------------------*/
    $.MouseoveE = function () {
        $(this).css("border-color", "#3174a6");
    };

    $.MouseoutE = function () {
        $(this).css("border-color", "transparent");
    };

    /*---------------------------
    小图标事件绑定  $.InitEvent
    ---------------------------*/
    $.InitEvent = function () {
        $.Grid.find("tr.tail div[name=tailfirst]").live("click", $.GoToFirstPage)
            .live("mouseover", $.MouseoveE).live("mouseout", $.MouseoutE);
        $.Grid.find("tr.tail div[name=tailprev]").live("click", $.GoToPrePage)
            .live("mouseover", $.MouseoveE).live("mouseout", $.MouseoutE);
        $.Grid.find("tr.tail div[name=tailnext]").live("click", $.GoToNextPage)
            .live("mouseover", $.MouseoveE).live("mouseout", $.MouseoutE);
        $.Grid.find("tr.tail div[name='taillast']").live("click", $.GoToLastPage)
            .live("tr.tail mouseover", $.MouseoveE).live("mouseout", $.MouseoutE);
        $.Grid.find("tr.tail div[name=tailreload]").live("click", $.ReLoadThisPage)
            .live("mouseover", $.MouseoveE).live("mouseout", $.MouseoutE);
    };

    /*---------------------------
    请求第一页 GoToFirstPage
    ---------------------------*/
    $.GoToFirstPage = function () {
        var pager = $.extend({}, $.Pager);
        pager.pageNumber = 1;
        $.AjaxGetInfo(pager);
    };

    /*---------------------------
    请求当前页的上一页 GoToFirstPage
    ---------------------------*/
    $.GoToPrePage = function () {
        var pageNumber = parseInt($.Grid.find("tr.tail div span[name=pageNumber]").text());
        var pageCount = parseInt($.Grid.find("tr.tail div span[name=pageCount]").text());
        pageNumber--;
        if (pageNumber <= pageCount && pageNumber > 0) {
            var pager = $.extend({}, $.Pager);
            pager.pageNumber = pageNumber;
            $.AjaxGetInfo(pager);
        }
    };

    /*---------------------------
    请求当前页的下一页 GoToNextPage
    ---------------------------*/
    $.GoToNextPage = function () {
        var pageNumber = parseInt($.Grid.find("tr.tail div span[name=pageNumber]").text());
        var pageCount = parseInt($.Grid.find("tr.tail div span[name=pageCount]").text());
        pageNumber++;
        if (pageNumber <= pageCount && pageNumber > 0) {
            var pager = $.extend({}, $.Pager);
            pager.pageNumber = pageNumber;
            $.AjaxGetInfo(pager);
        }
    };

    /*---------------------------
    请求最后一页 GoToLastPage
    ---------------------------*/
    $.GoToLastPage = function () {
        var pageCount = parseInt($.Grid.find("tr.tail div span[name=pageCount]").text());
        if (pageCount > 0) {
            var pager = $.extend({}, $.Pager);
            pager.pageNumber = pageCount;
            $.AjaxGetInfo(pager);
        }
    };

    /*---------------------------
    重新请求当前页 ReLoadPage
    ---------------------------*/
    $.ReLoadThisPage = function () {
        var pageNumber = parseInt($.Grid.find("tr.tail div span[name=pageNumber]").text());
        var pageCount = parseInt($.Grid.find("tr.tail div span[name=pageCount]").text());
        if (pageNumber <= pageCount && pageNumber > 0) {
            var pager = $.extend({}, $.Pager);
            pager.pageNumber = pageNumber;
            $.AjaxGetInfo(pager);
        }
    };

    /*---------------------------
    调用ajax请求
    ---------------------------*/
    $.AjaxGetInfo = function (pager) {
        $.Grid.find("tr.tail div[name=loadgif]").show();
        $.Grid.addClass("flyingGridCover");
        $.ajax({
            type: $.Cfg.type,
            cache: $.Cfg.cache,
            data: $.extend(pager, $.Cfg.data),
            url: $.Cfg.url,
            success: function (data) {
                $.GridContainer.html(data);
                $.Grid = $.GridContainer.find("#" + $.GridContainer.attr("grid"));
                $.UsePage();
                $.Grid.find("tr.tail div[name=loadgif]").hide();
                $.Grid.removeClass("flyingGridCover");
            },
            error: function () {
                $.Grid.find("tr.tail div[name=loadgif]").hide();
                $.Grid.find("tr.tail div[name=err]").text("请求错误，请重试！").show();
            }
        });
    };

    $.UsePage = function () {
        if ($.Grid.attr("usePager") == "true") {
            var trTail = new Array();
            trTail[0] = "<tr class=tail><td colspan=" + $.Grid.attr("colCount") + ">";
            trTail[1] = "<div name=tailfirst></div><div name=tailprev></div><div class=line></div>";
            trTail[2] = "<div name=tailinput><span name=pageNumber>" + $.Grid.attr("pageNumber") + "</span>/";
            trTail[3] = "<span name=pageCount>" + $.Grid.attr("pageCount") + "</span></div>";
            trTail[4] = "<div class=line></div><div name=tailnext></div><div name=taillast></div>";
            trTail[5] = "<div class=line></div><div name=tailreload></div><div class=line></div>";
            trTail[6] = "<div name=tailMsg>总计" + $.Grid.attr("dataCount") + "条...</div>";
            trTail[7] = "<div name=loadgif></div><div name=err></div></td></tr>";
            if ($.Grid.find("tr:last").attr("class") === "tail") {
                $.Grid.find("tr:last").remove();
            }
            $.Grid.find("tr:last").after(trTail.join(""));
        }
    };
})(jQuery);