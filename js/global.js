$(document).ready(function() {
    $.ajax({
      dataType: 'jsonp',
      jsonpCallback: 'ghjsonp',
      crossDomain: true,
      url: 'https://api.github.com/repos/SirCmpwn/Craft.Net/commits',
      success: function (jsonp) {
            var data = jsonp.data;
            var ul = "<ul>";
            for (var i = 0; i < 5 && i < data.length; i++)
            {
                data[i].sha = data[i].sha.substring(0,10);
                if (i == 0)
                    ul += '<li class="visible"><a class="shorten" href="https://github.com/SirCmpwn/Craft.Net/commit/' + data[i].sha + '">' + data[i].sha + '</a>' +
                    data[i].commit.message + ' by <a href="http://github.com/' + data[i].author.login + '">' + data[i].author.login + '</a>';
                else
                    ul += '<li><a href="https://github.com/SirCmpwn/Craft.Net/commit/' + data[i].sha + '">' + data[i].sha + '</a>' +
                    data[i].commit.message +
                    ' by <a href="http://github.com/' + data[i].author.login + '">' + data[i].author.login + '</a>';
            }
            ul += "</ul>";
            $("#commits").html(ul);
            setTimeout(scrollNext, 3000);
        }
    });
});

function scrollNext() {
    var next = $('#commits li.visible').next();
    if ($('#commits li.visible').index() + 1 == $('#commits ul').children().length)
        next = $('#commits ul').children().first();
    $('#commits li.visible').fadeOut(250, function () {
        $('#commits li.visible').removeClass('visible');
        next.addClass('visible');
        next.fadeIn(250);
    });
    
    setTimeout(scrollNext, 3000);
}