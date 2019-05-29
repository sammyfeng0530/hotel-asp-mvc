/*!
 * jquery.slide v1.1.0
 * A simple jQuery slider.
 * https://github.com/cobish/jquery.slide

 * Copyright (c) 2016, cobish
 * Released under the MIT license.
 */
(function($, window) {
  'use strict';

  var slide = {
    // default options
    defaults: {
      isAutoSlide: true,                // 自动轮播
      isHoverStop: true,                // 鼠标移上是否停止轮播
      isBlurStop: true,                 // Window失去焦点是否停止轮播
      isShowDots: true,                 // 是否显示状态点
      isShowArrow: true,                // 是否显示左右箭头
      isHoverShowArrow: true,           // 是否鼠标移上才显示箭头
      isLoadAllImgs: false,             // 是否一次性加载完全部图片
      slideSpeed: 10000,                // 轮播速度 (ms)
      switchSpeed: 500,                 // 图片切换速度 (ms)
      dotsClass: 'dots',                // 状态点样式
      dotActiveClass: 'active',         // 状态点激活样式
      dotsEvent: 'click',               // 状态点事件，click或mouseover或mouseenter
      arrowClass: 'arrow',              // 箭头样式
      arrowLeftClass: 'arrow-left',     // 左箭头样式
      arrowRightClass: 'arrow-right'    // 右箭头样式
    },

    // curr options
    options: null,

    // 当前索引
    curIndex: 0,

    // 定时器
    timer: null,

    // 状态点集合
    dotsList: [],

    // init function
    init: function(elem, options) {
      var $self = $(elem),
          list = $self.find('ul li'),
          self = this,
          o;

      o = this.options = $.extend({}, this.defaults, options);

      // 显示状态点
      if (o.isShowDots) {
        this._createDots(elem, list);
      }

      // 显示左右箭头
      if (o.isShowArrow) {
        this._createArrow(elem, list);
      }

      // 一次性加载完全部图片
      if (o.isLoadAllImgs) {
        list.each(function() {
          $(this).css({
            'background': 'url(' + $(this).attr('data-bg') + ')',
            'opacity': '0',
            'z-index': '0',
              'background-position': '50% 50%',
  'background-size': 'cover'
          });
          $(this).attr('data-bg', '');
        });
      }

      // 显示第一个
      this._showBlock(list[this.curIndex]);

      // 自动轮播
      if (o.isAutoSlide) {
        this._defaultSlide(list);

        // 鼠标移入移除事件
        if (o.isHoverStop) {
          var className = $self.attr('class');
          $self.on('mouseenter', function(e) {
            clearInterval(self.timer);
          }).on('mouseleave', function() {
            clearInterval(self.timer);
            self._defaultSlide(list);
          });
        }

        // Window获取失去焦点事件
        if (o.isBlurStop) {
          $(window).on('blur', function() {
            clearInterval(self.timer);
          }).on('focus', function() {
            clearInterval(self.timer);
            self._defaultSlide(list);
          });
        }
      }
    },

    // default slide
    _defaultSlide: function(list) {
      var self = this;
      this.timer = setInterval(function() {
        self._hideBlock(list[self.curIndex]);
        self.curIndex =  (self.curIndex + 1) % list.length;
        self._showBlock(list[self.curIndex]);
      }, this.options.slideSpeed);
    },

    // show block
    _showBlock: function(block) {
      var o = this.options,
          $block = $(block),
          bg = $(block).attr('data-bg');

      if (bg) {
        $block.css({
          'background': 'url(' + bg + ')',
          'opacity': '0',
          'z-index': '0',
          'background-position': '50% 50%',
  'background-size': 'cover'
        });
        $block.attr('data-bg', '');
      }

      $block.stop().animate({
        'opacity': '1',
        'z-index': '1'
      }, o.switchSpeed);

      if (o.isShowDots) {
        $(this.dotsList[this.curIndex]).addClass(o.dotActiveClass);
      }
    },

    // hide block
    _hideBlock: function(block) {
      var o = this.options;

      $(block).stop().animate({
        'opacity': '0',
        'z-index': '0'
      }, o.switchSpeed);

      if (o.isShowDots) {
        $(this.dotsList[this.curIndex]).removeClass(o.dotActiveClass);
      }
    },

    // create dots
    _createDots: function(elem, list) {
      var self = this,
          dotsEvent = this.options.dotsEvent;

      var dots = $('<ol/>', {
        class: this.options.dotsClass
      });

      var dotsList = [];
      for (var i = 0; i < list.length; i++) {
        dotsList[i] = $('<li/>');
        dots.append(dotsList[i]);
      }

      $(elem).append(dots);
      this.dotsList = dotsList;

      // dots添加事件
      if (dotsEvent === 'click' || dotsEvent === 'mouseover' || dotsEvent === 'mouseenter') {
        dots.find('li').on(dotsEvent, function() {
          self._hideBlock(list[self.curIndex]);
          self.curIndex =  $(this).index();
          self._showBlock(list[self.curIndex]);
        });
      }
    },

    // create arrowClass
    _createArrow: function(elem, list) {
      var self = this;

      var arrow = $('<div/>', {
        class: this.options.arrowClass
      });

      var leftArrow = $('<a/>', {
        class: this.options.arrowLeftClass
      });

      var rightArrow = $('<a/>', {
        class: this.options.arrowRightClass
      });

      arrow.append(leftArrow).append(rightArrow);
      $(elem).append(arrow);

      if (this.options.isHoverShowArrow) {
        arrow.css('opacity', 0);
        $(elem).on('mouseenter', function() {
          arrow.css('opacity', 1);
        }).on('mouseleave', function() {
          arrow.css('opacity', 0);
        });
      }

      leftArrow.on('click', function() {
        self._hideBlock(list[self.curIndex]);
        self.curIndex = (list.length + (self.curIndex - 1)) % list.length;
        self._showBlock(list[self.curIndex]);
      });

      rightArrow.on('click', function() {
        self._hideBlock(list[self.curIndex]);
        self.curIndex =  (self.curIndex + 1) % list.length;
        self._showBlock(list[self.curIndex]);
      });
    }
  };

  // main function
  $.fn.slide = function(options) {
    slide.init(this, options);
    return this;
  };

})(jQuery, window);
