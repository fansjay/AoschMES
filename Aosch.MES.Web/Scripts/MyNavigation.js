function changeFrameHeight(frameid) {

    var ifm = document.getElementById(frameid);
    if (ifm != null) {
        ifm.height = document.documentElement.clientHeight - 100;
    }

}
window.onresize = function () {
    changeFrameHeight("AoschFrame1");
}

//获取cookie
function getCookie(cname) {
    var name = cname + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1);
        if (c.indexOf(name) != -1) return c.substring(name.length, c.length);
    }
    return "";
}


function iFrameHeight() {
    var ifm = document.getElementById("iframepage");
    var subWeb = document.frames ? document.frames["iframepage"].document : ifm.contentDocument;
    if (ifm != null && subWeb != null) {
        ifm.height = subWeb.body.scrollHeight;
        ifm.width = subWeb.body.scrollWidth;
    }
}
$(function () {
    $("#userInfo").css("display", "none");
   
    //$("#signuser").html('欢迎您,' + getCookieCN('username'));
    //var userid = getCookie('userid');
    getNavigationFansjay(getCookie("Username"));
});



function showMenu() {
    $(".tabs-inner").bind('contextmenu', function (e) {
        $('#mm').menu('show', { left: e.pageX, top: e.pageY });
        var subtitle = $(this).children(".tabs-closable").text();
        $('#mm').data("currtab", subtitle);
        $('#tt').tabs('select', subtitle);
        return false;
    });
}
function tabEvent() {
    //刷新
    $('#mm-tabupdate').click(function () {
        var currTab = $('#tt').tabs('getSelected');
        var url = $(currTab.panel('options').content).attr('src');
        if (url != undefined) {
            var content = '<iframe scrolling="auto" frameborder="0"  src="' + url + '" style="width:100%;height:100%;"></iframe>';
            $('#tt').tabs('update', { tab: currTab, options: { content: content} });
        }
    });
    //关闭当前
    $('#mm-tabclose').click(function () {
        var currtab_title = $('#mm').data("currtab");
        $('#tt').tabs('close', currtab_title);
    });
    //关闭所有
    $('#mm-tabcloseall').click(function () {
        $('.tabs-inner span').each(function (i, n) {
            var t = $(n).text();
            if (t != '首页') {
                $('#tt').tabs('close', t);
            }
        });
    });
    //关闭除当前之外的TAB
    $('#mm-tabcloseother').click(function () {
        var prevall = $('.tabs-selected').prevAll();
        var nextall = $('.tabs-selected').nextAll();
        if (prevall.length > 0) {
            prevall.each(function (i, n) {
                var t = $('a:eq(0) span', $(n)).text();
                if (t != '首页')
                   $('#tt').tabs('close', t);
            });
        }
        if (nextall.length > 0) {
            nextall.each(function (i, n) {
                var t = $('a:eq(0) span', $(n)).text();
                if (t != '首页')
                   $('#tt').tabs('close', t);
            });
        }
        return false;
    });
}
//testdemo
function getNavigationFansjay(username) {
    //var rolename = getCookieCN('rolename');
    //var Username = 'admin';
    $.getJSON("/Home/GetNavigationsFansjay?Username=" + username, function (data) {
        var FirstLevelHtml = "";
        $.each(data, function (i, item) {
            if (item != null) {
                FirstLevelHtml = FirstLevelHtml + "<li class='treeview' onclick='onMenuItemClick(this)'><a href='" + item.URL + "'><i class='" + item.Icon + " text-aqua'></i><span class='pull-right-container'><i class='fa fa-angle-left pull-right'></i></span> <span>" + item.MenuName + "</span></a>";
            }
          
            //console.log(item.attributes.iconCls);
            if (item.Childrens != null) {//有下级菜单 二级菜单
                FirstLevelHtml = FirstLevelHtml + "<ul class='treeview-menu'>";
                $.each(item.Childrens, function (j, seconditem) {//二级菜单
                        
                    var AddRightIcon = "";
                    var LinkHref = " onclick='onMenuItemClick(this)' ";
                    if (seconditem.Childrens != null) {
                        LinkHref = ""; 
                        //展开图标
                        AddRightIcon = "<span class='pull-right-container'><i class='fa fa-angle-left pull-right'></i></span>";
                    }
                    //console.log(seconditem);
                    FirstLevelHtml = FirstLevelHtml + "<li class='treeview'><a href='javascript:void(0)'  data-title='" + seconditem.MenuName + "' data-href='" + seconditem.URL + "'  " + LinkHref + " ><i class=' " + seconditem.Icon + " text-red'></i> " + AddRightIcon + "" + seconditem.MenuName + "</a>";
                    if (seconditem.Childrens != null) {
                        FirstLevelHtml = FirstLevelHtml + "<ul class='treeview-menu'>";
                        $.each(seconditem.Childrens, function (k, thirditem) {//三级菜单
                            FirstLevelHtml = FirstLevelHtml + "<li><a href='javascript:void(0)' data-title='" + thirditem.MenuName + "' data-href='" + thirditem.URL + "'  onclick=' onMenuItemClick(this)'><i class='" + thirditem.Icon + " text-green'></i>" + thirditem.MenuName + "</a></li>";
                        });
                        FirstLevelHtml = FirstLevelHtml + "</ul>";
                    }
                    FirstLevelHtml = FirstLevelHtml + "</li>"
                })
                FirstLevelHtml = FirstLevelHtml + "</ul>";
            }
            FirstLevelHtml = FirstLevelHtml + "</li>"
        });
  
        //一级菜单
        $("#ulFirstLevel").append(function () {
            return FirstLevelHtml;
        });
      
    });
}
//计算元素集合的总宽度
function calSumWidth(elements) {
    var width = 0;
    $(elements).each(function () {
        width += $(this).outerWidth(true);
    });
    return width;
}
//滚动到指定选项卡
function scrollToTab(element) {
    var marginLeftVal = calSumWidth($(element).prevAll()), marginRightVal = calSumWidth($(element).nextAll());
    // 可视区域非tab宽度
    var tabOuterWidth = calSumWidth($(".content-tabs").children().not(".J_menuTabs"));
    //可视区域tab宽度
    var visibleWidth = $(".content-tabs").outerWidth(true) - tabOuterWidth;
    //实际滚动宽度
    var scrollVal = 0;
    if ($(".page-tabs-content").outerWidth() < visibleWidth) {
        scrollVal = 0;
    } else if (marginRightVal <= (visibleWidth - $(element).outerWidth(true) - $(element).next().outerWidth(true))) {
        if ((visibleWidth - $(element).next().outerWidth(true)) > marginRightVal) {
            scrollVal = marginLeftVal;
            var tabElement = element;
            while ((scrollVal - $(tabElement).outerWidth()) > ($(".page-tabs-content").outerWidth() - visibleWidth)) {
                scrollVal -= $(tabElement).prev().outerWidth();
                tabElement = $(tabElement).prev();
            }
        }
    } else if (marginLeftVal > (visibleWidth - $(element).outerWidth(true) - $(element).prev().outerWidth(true))) {
        scrollVal = marginLeftVal - $(element).prev().outerWidth(true);
    }
    $('.page-tabs-content').animate({
        marginLeft: 0 - scrollVal + 'px'
    }, "fast");
}
//查看左侧隐藏的选项卡
function scrollTabLeft() {
    var marginLeftVal = Math.abs(parseInt($('.page-tabs-content').css('margin-left')));
    // 可视区域非tab宽度
    var tabOuterWidth = calSumWidth($(".content-tabs").children().not(".J_menuTabs"));
    //可视区域tab宽度
    var visibleWidth = $(".content-tabs").outerWidth(true) - tabOuterWidth;
    //实际滚动宽度
    var scrollVal = 0;
    if ($(".page-tabs-content").width() < visibleWidth) {
        return false;
    } else {
        var tabElement = $(".J_menuTab:first");
        var offsetVal = 0;
        while ((offsetVal + $(tabElement).outerWidth(true)) <= marginLeftVal) {//找到离当前tab最近的元素
            offsetVal += $(tabElement).outerWidth(true);
            tabElement = $(tabElement).next();
        }
        offsetVal = 0;
        if (calSumWidth($(tabElement).prevAll()) > visibleWidth) {
            while ((offsetVal + $(tabElement).outerWidth(true)) < (visibleWidth) && tabElement.length > 0) {
                offsetVal += $(tabElement).outerWidth(true);
                tabElement = $(tabElement).prev();
            }
            scrollVal = calSumWidth($(tabElement).prevAll());
        }
    }
    $('.page-tabs-content').animate({
        marginLeft: 0 - scrollVal + 'px'
    }, "fast");
}
//查看右侧隐藏的选项卡
function scrollTabRight() {
    var marginLeftVal = Math.abs(parseInt($('.page-tabs-content').css('margin-left')));
    // 可视区域非tab宽度
    var tabOuterWidth = calSumWidth($(".content-tabs").children().not(".J_menuTabs"));
    //可视区域tab宽度
    var visibleWidth = $(".content-tabs").outerWidth(true) - tabOuterWidth;
    //实际滚动宽度
    var scrollVal = 0;
    if ($(".page-tabs-content").width() < visibleWidth) {
        return false;
    } else {
        var tabElement = $(".J_menuTab:first");
        var offsetVal = 0;
        while ((offsetVal + $(tabElement).outerWidth(true)) <= marginLeftVal) {//找到离当前tab最近的元素
            offsetVal += $(tabElement).outerWidth(true);
            tabElement = $(tabElement).next();
        }
        offsetVal = 0;
        while ((offsetVal + $(tabElement).outerWidth(true)) < (visibleWidth) && tabElement.length > 0) {
            offsetVal += $(tabElement).outerWidth(true);
            tabElement = $(tabElement).next();
        }
        scrollVal = calSumWidth($(tabElement).prevAll());
        if (scrollVal > 0) {
            $('.page-tabs-content').animate({
                marginLeft: 0 - scrollVal + 'px'
            }, "fast");
        }
    }
}

//添加tab
function AddTabFansjay(URL, Text) {
    //添加TAB
    flag = true;
    var URL = URL;
    Title = Text;
    //选项卡菜单已存在
    $('.J_menuTab').each(function () {

        if ($(this).data("id") == URL) {
            //alert("已经存在");
            if (!$(this).hasClass('active')) {
                $(this).addClass('active').siblings('.J_menuTab').removeClass('active');
                scrollToTab(this);
                // 显示tab对应的内容区
                $('.J_mainContent .J_iframe').each(function () {
                    if ($(this).data('id') == URL) {
                        $(this).show().siblings('.J_iframe').hide();
                        return false;
                    }
                });
            }
            flag = false;
            return false;
        }
    });

    // 选项卡菜单不存在
    if (flag) {
        var str = '<a href="javascript:;" class="active J_menuTab" data-id="' + URL + '">' + Title + ' <i class="fa fa-times-circle"></i></a>';
        $('.J_menuTab').removeClass('active');
        var frameid = URL.replace('/', '');
        frameid = frameid.replace('/', '');
        // 添加选项卡对应的iframe
        var str1 = '<iframe class="J_iframe" scrolling="auto"  name="' + frameid + '" id="' + frameid + '" onload="changeFrameHeight(\'' + frameid + '\')"  width="100%" height="100%" src="' + URL + '" frameborder="0" data-id="' + URL + '" seamless></iframe>';
        $('.J_mainContent').find('iframe.J_iframe').hide().parents('.J_mainContent').append(str1);
        $('.J_menuTabs .page-tabs-content').append(str);
        scrollToTab($('.J_menuTab.active'));
    }

}

//通过遍历给菜单项加上data-index属性
$(".J_menuItem").each(function (index) {
    if (!$(this).attr('data-index')) {
        $(this).attr('data-index', index);
    }
});
function onMenuItemClick(a) {
    var Flevel = a.MenuName;
    var breadcrumpstring = "";
  
    //添加TAB
    flag = true;
    var URL = a.dataset.href;
    //var URL = $(a).attr("data-href")
    //Title = a.MenuName

    if (URL == null || URL=="") return;
    Title = $(a).text();
     //选项卡菜单已存在
    $('.J_menuTab').each(function () {
        if ($(this).data("id") == URL) {
            //alert("已经存在");
            if (!$(this).hasClass('active')) {
                $(this).addClass('active').siblings('.J_menuTab').removeClass('active');
                scrollToTab(this);
                // 显示tab对应的内容区
                $('.J_mainContent .J_iframe').each(function () {
                    if ($(this).data('id') == URL) {
                        $(this).show().siblings('.J_iframe').hide();
                        return false;
                    }
                });
            }
            flag = false;
            return false;
        }
    });

    // 选项卡菜单不存在
    if (flag) {
        var str = '<a href="javascript:;" class="active J_menuTab" data-id="' + URL + '">' + Title + ' <i class="fa fa-times-circle"></i></a>';
        $('.J_menuTab').removeClass('active');
        var frameid = URL.replace('/', '');
        frameid = frameid.replace('/', '');
        // 添加选项卡对应的iframe
        var str1 = '<iframe class="J_iframe" scrolling="auto"  name="' + frameid + '" id="' + frameid + '" onload="changeFrameHeight(\'' + frameid + '\')"  width="100%" height="100%" src="' + URL + '" frameborder="0" data-id="' + URL + '" seamless></iframe>';
        $('.J_mainContent').find('iframe.J_iframe').hide().parents('.J_mainContent').append(str1);
        $('.J_menuTabs .page-tabs-content').append(str);
        scrollToTab($('.J_menuTab.active'));
    }
}
function onNodeClick(node) {
    var id = this.ID;
    if (!$('#' + id).tree('IsLeaf', node.target)) {
        $('#' + id).tree('toggle', node.target);
    }
    else {
        addTab(node.URL + "?navigationId=" + node.id, node.MenuName, true);
        //addTab(node.attributes.url, node.text);
    }
}
function userInfo() {
    
    if (getCookieCN('userid') == null) {
        layer.alert("没有登录");
        return false;
    }
    
    var userid= getCookieCN('userid');
    var username = getCookieCN('username');
    var register = getCookieCN('register');
   var registertime = getCookieCN('registetime');
    var department = getCookieCN('department');
    var rolename = getCookieCN('rolename');
   var position = getCookieCN('position');
   var telphone= getCookieCN('phone') == null ? "" : getCookieCN('phone');
    var email = getCookieCN('email') == null ? "" : getCookieCN('email');
    var linkaddress = getCookieCN('linkaddress') == null ? "" : getCookieCN('linkaddress');
    $("#realname").val(username); $("#rolename").val(rolename);
   
    layer.open({
        type: 1,
        skin: 'layui-layer-rim', //加上边框
        area: ['600px', '240px'], //宽高
        content: '<section><div class="container" style="padding-top:30px;color:black; boder:1px solid gray;"><div class="row"><div class="form-group"><label for="inputSkills" class="col-sm-3 control-label">用户名：</label><label for="inputSkills" class="col-sm-3 control-label">' + username + '</label><label for="inputSkills" class="col-sm-3 control-label">注册时间：</label><label for="inputSkills" class="col-sm-3 control-label">' + registertime + '</label></div></div>' +
                  '<div class="row">'+
                       ' <div class="form-group">'+
                          '   <label for="inputSkills" class="col-sm-3 control-label">部&nbsp;&nbsp;  门：</label>' +
                           '  <label for="inputSkills" class="col-sm-3 control-label">' + department + '</label>' +
                           '  <label for="inputSkills" class="col-sm-3 control-label">角&nbsp;&nbsp; &nbsp;&nbsp; 色：</label>' +
                            ' <label for="inputSkills" class="col-sm-3 control-label">'+rolename+'</label>'+
                      '   </div>'+
                   '  </div>'+
                    ' <div class="row">'+
                      '   <div class="form-group">'+
                            ' <label for="inputSkills" class="col-sm-3 control-label">职&nbsp;&nbsp;  位：</label>' +
                            ' <label for="inputSkills" class="col-sm-3 control-label">' + position + '</label>' +
                            ' <label for="inputSkills" class="col-sm-3 control-label">电&nbsp; &nbsp;&nbsp;&nbsp; 话：</label>' +
                            ' <label for="inputSkills" class="col-sm-3 control-label">' + telphone + '</label>' +
                        ' </div>'+
                    ' </div>'+
                    ' <div class="row">'+
                      '   <div class="form-group">'+
                            ' <label for="inputSkills" class="col-sm-3 control-label">邮&nbsp;&nbsp;  箱：</label>' +
                            ' <label for="inputSkills" class="col-sm-3 control-label">' + email + '</label>' +
                          '   <label for="inputSkills" class="col-sm-3 control-label">链 &nbsp;&nbsp;&nbsp;&nbsp; 接：</label>' +
                          '   <label for="inputSkills" class="col-sm-3 control-label">' + linkaddress + '</label>' +
                       '  </div>'+
                   '  </div>'+
                ' </div>' +

            ' </section>'
    });

  
}
function UpdatePassword() {
    layer.open({
        type: 1,
        skin: 'layui-layer-rim', //加上边框
        area: ['580px', '380px'], //宽高
        content: '<section><div class="container" style="padding:30px; boder:1px solid gray;color:#000;">'+
                  '<div class="row">' +
                       ' <div class="form-group">' +
                          '   <label for="inputSkills" class="col-sm-3 control-label">密&nbsp;&nbsp;  码：</label>' +
                           '   <input class="form-control" id="pwd" name="pwd" type="password">' +
                      '   </div>' +
                   '  </div>' +
                    ' <div class="row">' +
                      '   <div class="form-group">' +
                            ' <label for="inputSkills" class="col-sm-3 control-label">新密码：</label>' +
                            '   <input class="form-control" id="pwd0" name="pwd" type="password">' +
                        ' </div>' +
                    ' </div>' +
                       ' <div class="row">' +
                      '   <div class="form-group">' +
                            ' <label for="inputSkills" class="col-sm-3 control-label">确&nbsp;&nbsp;  认：</label>' +
                             '   <input class="form-control" id="pwd1" name="pwd" type="password">' +
                        ' </div>' +
                    ' </div>' +
                    ' <div class="row">' +
                      '   <div class="form-group">' +
                        '   <button type="submit" onclick="savepassword()" class="btn btn-primary btn-flat">修改密码</button>'+
                       '  </div>' +
                   '  </div>' +
                ' </div>' +

            ' </section>'
    });
}

//执行修改密码请求
    function savepassword() {
      var userid = getCookieCN('userid');
      var pwd = document.getElementById('pwd').value;
      var pwd0 = document.getElementById('pwd0').value;
      var pwd1 = document.getElementById('pwd1').value;
      if (pwd == "") {
          layer.msg("原始密码不能为空!", { icon: 2 }, 2000);
          return;
      }
      if (pwd0 != pwd1) {
          layer.msg("两次密码不相同", { icon: 2 }, 2000);
          return;
      }
      $.post('/Login/UpdatePassword', { userid: userid, pwd: pwd, pwd0: pwd0 }, function (data) {
         if (data.Status=="OK") {
              layer.msg("密码修改成功!请重新登录", { icon: 1 }, 1000);
              if (window != window.parent) {
                  window.parent.location.reload();
              }
              top.window.location.href = "/Login/Index";
         } else {
             layer.msg("密码修改失败!", { icon: 2 }, 2000);
         }
      });
  }
     //退出当前用户
    function Exit() {
        //清除所有cookie
        var keys = document.cookie.match(/[^ =;]+(?=\=)/g);
       // console.log(keys);
        if (keys) {
            for (var i = keys.length; i--;)
                document.cookie = keys[i] + '=0;expires=' + new Date(0).toUTCString()+";path=/"
        }
        //跳转到登录界面 
        window.location.href = "/Login/Index";
    }
