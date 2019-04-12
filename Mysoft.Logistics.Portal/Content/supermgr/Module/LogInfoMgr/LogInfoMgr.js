var SchedulerLogMgr = function () {
    var loadUrl = "";
    var exports = {};
    exports.options = {};
    exports.options.JobId = $.fn.request('keyValue');
    //任务类型
    exports.options.JobStates = {
        1: "label label-success",
        2: "label label-danger",
        3: "label label-warning"
    }
    //初始化数据
    exports.initGridPage = function () {

        //初始化
        $(window).resize(function (e) {
            window.setTimeout(function () {

                $('#gridTable').setGridWidth(($('.gridPanel').width()));
                $("#gridTable").setGridHeight($.fn.getGridHeight());

            }, 100);
            e.stopPropagation();
        });

        //查询条件
        $("#queryCondition .dropdown-menu li").click(function () {
            var text = $(this).find('a').html();
            var value = $(this).find('a').attr('data-value');
            $("#queryCondition .dropdown-text").html(text).attr('data-value', value);
        });

        //查询回车
        $('#txt_Keyword').bind('keypress', function (event) {
            if (event.keyCode == "13") {
                $('.lr-search').trigger("click");
            }
        });

        //注册事件
        $(".titlePanel").on("click", ".lr-search,.lr-view", function () {
            var $this = $(this);
            if ($this.hasClass('lr-search')) {
                exports.SearchEvent();
            }
            else if ($this.hasClass('lr-view')) {
                exports.btnViewLogDetail();
            };
        });
    };


    //查看任务日志详情
    exports.btnViewLogDetail = function () {
        var keyValue = $("#gridTable").jqGridRowValue("upgrade_log_id");
        if (checkedRow(keyValue)) {
            $.fn.modalOpen({
                id: "LogDetailForm",
                title: '查看日志详情',
                url: '/Log/Detail/' + keyValue,
                width: "800px",
                height: "600px",
                callBack: function (iframeId) {

                }
            });
        }
    }

    //加载Grid
    exports.loadGrid = function (url) {
        var selectedRowIndex = 0;
        loadUrl = url;
        exports.options.$gridTable = $("#gridTable");
        exports.options.$gridTable.jqGrid({
            datatype: "json",
            mtype: "POST",
            url: url + exports.options.JobId,
            height: $.fn.getGridHeight(),
            autowidth: true,
            colModel: [
                { label: "主键", name: "upgrade_log_id", index: "upgrade_log_id", hidden: true },
                { label: "序号", name: "displayid", hidden: true },
                { label: "部署点名称", name: "server_name", index: "server_name", width: 180, align: "left" },
                { label: "名称", name: "name", index: "name", width: 180, align: "left" },
                { label: "升级时间", name: "upgrade_date", index: "upgrade_date", width: 180, align: "left" },
                { label: "日志类型", name: "log_type", index: "log_type", width: 180, align: "left" },
                { label: "日志内容", name: "log_content", index: "log_content", width: 180, align: "left" },
                { label: "创建日期", name: "create_date", index: "create_date", width: 180, align: "left" }
            ],
            pager: "#gridPager",
            sortname: 'end_date',
            sortorder: "desc",
            rowList: [20, 50, 100, 500, 1000],
            rowNum: "20",
            rownumbers: true,
            onSelectRow: function () {
                selectedRowIndex = $("#" + this.id).getGridParam('selrow');
            },
            gridComplete: function () {
                $("#" + this.id).setSelection(selectedRowIndex, false);
            },
            loadError: function (xhr, status, error) {
                switch (error) {
                    case "Unauthorized":
                        top.location.href = "/account/login";
                        break;
                    default:
                        $.fn.modalMsg(error, "error");
                        break;
                }
            }
        });
        exports.SearchEvent();
    }

    //查询表格函数
    exports.SearchEvent = function () {
        var queryJson = $("#form1").GetWebControls();
        queryJson["Type"] = $("#queryCondition .dropdown-text").attr('data-value');
        queryJson["StartTime"] = $("#StartTime").val();
        queryJson["EndTime"] = $("#EndTime").val();
        $("#gridTable").jqGrid('setGridParam', {
            url: loadUrl,
            postData: queryJson,
            page: 1
        }).trigger('reloadGrid');
    }

    //初始化表单
    exports.initFormControl = function (readonly) {
        exports.options.KeyValue = $.fn.request("keyValue");
        //获取表单
        if (!!exports.options.KeyValue) {
            $.fn.setForm({
                url: "/SysMgr/SchedulerMgr/GetJobLogEntity",
                param: { keyValue: exports.options.KeyValue },
                success: function (data) {
                    $("#form1").SetWebControls(data);

                    if (readonly) {
                        $("#form1").find('.form-control,.ui-select,input,textarea').attr('disabled', 'disabled');
                    }
                }
            });
        }
    }

    return exports;
};