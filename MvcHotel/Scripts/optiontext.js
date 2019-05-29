//计时器

var attime;
  var colorHead,colorFoot;
  function clock(){
    var time=new Date();
    var h=time.getHours();
    var m=time.getMinutes();
    var s=time.getSeconds();
    s<10 ? s="0"+s : s;
    m<10 ? m="0"+m : m;
    h<10 ? h="0"+h : h;
    var ww = ["星期日","星期一","星期二","星期三","星期四","星期五","星期六"]
    var nWeek = time.getDay();
    attime= h+":"+m+":"+s+" "+ww[nWeek];
    document.getElementById("clock").value = attime;
  }
  var timer = setInterval(clock,1000);