 AOS.init({
 	duration: 800,
 	easing: 'slide'
 });

(function($) {

	"use strict";

	$(window).stellar({
    responsive: true,
    parallaxBackgrounds: true,
    parallaxElements: true,
    horizontalScrolling: false,
    hideDistantElements: false,
    scrollProperty: 'scroll',
    horizontalOffset: 0,
	  verticalOffset: 0
  });

  // Scrollax
  $.Scrollax();


	var fullHeight = function() {

		$('.js-fullheight').css('height', $(window).height());
		$(window).resize(function(){
			$('.js-fullheight').css('height', $(window).height());
		});

	};
	fullHeight();

	// loader
	var loader = function() {
		setTimeout(function() { 
			if($('#ftco-loader').length > 0) {
				$('#ftco-loader').removeClass('show');
			}
		}, 1);
	};
	loader();

	// Scrollax
   $.Scrollax();

	var carousel = function() {
		$('.home-slider').owlCarousel({
	    loop:false,
	    autoplay: true,
	    margin:0,
	    animateOut: 'fadeOut',
	    animateIn: 'fadeIn',
	    nav:false,
	    autoplayHoverPause: false,
	    items: 1,
	    navText : ["<span class='ion-md-arrow-back'></span>","<span class='ion-chevron-right'></span>"],
	    responsive:{
	      0:{
	        items:1,
	        nav:false
	      },
	      600:{
	        items:1,
	        nav:false
	      },
	      1000:{
	        items:1,
	        nav:false
	      }
	    }
		});
		$('.carousel-work').owlCarousel({
			autoplay: true,
			center: true,
			loop: true,
			items:1,
			margin: 30,
			stagePadding:0,
			nav: true,
			navText: ['<span class="ion-ios-arrow-back">', '<span class="ion-ios-arrow-forward">'],
			responsive:{
				0:{
					items: 1,
					stagePadding: 0
				},
				600:{
					items: 2,
					stagePadding: 50
				},
				1000:{
					items: 3,
					stagePadding: 100
				}
			}
		});

	};
	carousel();

	$('nav .dropdown').hover(function(){
		var $this = $(this);
		// 	 timer;
		// clearTimeout(timer);
		$this.addClass('show');
		$this.find('> a').attr('aria-expanded', true);
		// $this.find('.dropdown-menu').addClass('animated-fast fadeInUp show');
		$this.find('.dropdown-menu').addClass('show');
	}, function(){
		var $this = $(this);
			// timer;
		// timer = setTimeout(function(){
			$this.removeClass('show');
			$this.find('> a').attr('aria-expanded', false);
			// $this.find('.dropdown-menu').removeClass('animated-fast fadeInUp show');
			$this.find('.dropdown-menu').removeClass('show');
		// }, 100);
	});


	$('#dropdown04').on('show.bs.dropdown', function () {
	  console.log('show');
	});

	// scroll
	var scrollWindow = function() {
		$(window).scroll(function(){
			var $w = $(this),
					st = $w.scrollTop(),
					navbar = $('.ftco_navbar'),
					sd = $('.js-scroll-wrap');

			if (st > 150) {
				if ( !navbar.hasClass('scrolled') ) {
					navbar.addClass('scrolled');	
				}
			} 
			if (st < 150) {
				if ( navbar.hasClass('scrolled') ) {
					navbar.removeClass('scrolled sleep');
				}
			} 
			if ( st > 350 ) {
				if ( !navbar.hasClass('awake') ) {
					navbar.addClass('awake');	
				}
				
				if(sd.length > 0) {
					sd.addClass('sleep');
				}
			}
			if ( st < 350 ) {
				if ( navbar.hasClass('awake') ) {
					navbar.removeClass('awake');
					navbar.addClass('sleep');
				}
				if(sd.length > 0) {
					sd.removeClass('sleep');
				}
			}
		});
	};
	scrollWindow();

	
	var counter = function() {
		
		$('#section-counter').waypoint( function( direction ) {

			if( direction === 'down' && !$(this.element).hasClass('ftco-animated') ) {

				var comma_separator_number_step = $.animateNumber.numberStepFactories.separator(',')
				$('.number').each(function(){
					var $this = $(this),
						num = $this.data('number');
						console.log(num);
					$this.animateNumber(
					  {
					    number: num,
					    numberStep: comma_separator_number_step
					  }, 7000
					);
				});
				
			}

		} , { offset: '95%' } );

	}
	counter();

	var contentWayPoint = function() {
		var i = 0;
		$('.ftco-animate').waypoint( function( direction ) {

			if( direction === 'down' && !$(this.element).hasClass('ftco-animated') ) {
				
				i++;

				$(this.element).addClass('item-animate');
				setTimeout(function(){

					$('body .ftco-animate.item-animate').each(function(k){
						var el = $(this);
						setTimeout( function () {
							var effect = el.data('animate-effect');
							if ( effect === 'fadeIn') {
								el.addClass('fadeIn ftco-animated');
							} else if ( effect === 'fadeInLeft') {
								el.addClass('fadeInLeft ftco-animated');
							} else if ( effect === 'fadeInRight') {
								el.addClass('fadeInRight ftco-animated');
							} else {
								el.addClass('fadeInUp ftco-animated');
							}
							el.removeClass('item-animate');
						},  k * 50, 'easeInOutExpo' );
					});
					
				}, 100);
				
			}

		} , { offset: '95%' } );
	};
	contentWayPoint();


	// navigation
	var OnePageNav = function() {
		$(".smoothscroll[href^='#'], #ftco-nav ul li a[href^='#']").on('click', function(e) {
		 	e.preventDefault();

		 	var hash = this.hash,
		 			navToggler = $('.navbar-toggler');
		 	$('html, body').animate({
		    scrollTop: $(hash).offset().top
		  }, 700, 'easeInOutExpo', function(){
		    window.location.hash = hash;
		  });


		  if ( navToggler.is(':visible') ) {
		  	navToggler.click();
		  }
		});
		$('body').on('activate.bs.scrollspy', function () {
		  console.log('nice');
		})
	};
	OnePageNav();


	// magnific popup
	$('.image-popup').magnificPopup({
    type: 'image',
    closeOnContentClick: true,
    closeBtnInside: true,
    fixedContentPos: true,
    mainClass: 'mfp-no-margins mfp-with-zoom', // class to remove default margin from left and right side
     gallery: {
      enabled: true,
      navigateByImgClick: true,
      preload: [0,1] // Will preload 0 - before current, and 1 after the current image
    },
    image: {
      verticalFit: true
    },
    zoom: {
      enabled: true,
      duration: 300 // don't foget to change the duration also in CSS
    }
  });

  $('.popup-youtube, .popup-vimeo, .popup-gmaps').magnificPopup({
    disableOn: 700,
    type: 'iframe',
    mainClass: 'mfp-fade',
    removalDelay: 160,
    preloader: false,

    fixedContentPos: false
  });

})(jQuery);

function convertCurrency(currency) {
	return currency.replace(/[^\d]/g, "");
}

function formatCurrency(number) {
	return number.toLocaleString("vi-VN", { style: "currency", currency: "VND" });
}

//Load product by category
function loadProduct(thisitem) {

	var activeCategory = document.querySelector(".nav-link.active");

	if (thisitem != null) {
		activeCategory.classList.remove("active");
		thisitem.classList.add("active");
		activeCategory = document.querySelector(".nav-link.active");
	}

	var searchbar = document.querySelector(".search-bar");

	var productRows = document.getElementsByClassName("product-item");
	var categoryId = activeCategory.getAttribute("data-categoryid");

	for (var i = 0; i < productRows.length; i++) {
		var row = productRows[i];
		var category = row.getAttribute("data-category");

		var productName = row.querySelector(".product-name").innerHTML;

		if ((categoryId == 0 || category == null
			|| (categoryId == -1 && row.getAttribute("data-favorite").toLowerCase() == "true")
			|| category == categoryId)
			&& Diacritics.clean(productName).toLowerCase().includes(Diacritics.clean(searchbar.value).toLowerCase())) {
			row.style.display = "";
		} else {
			row.style.display = "none";
		}
	}
}

function showToast(message) {
	Toastify({
		text: message,
		duration: 3000,
		close: true,
		style: {
			background: "linear-gradient(to right, #6f4e37, #b28451)",
		},
	}).showToast();
}

document.getElementById("call_staff").addEventListener('click', e => {
	$.confirm({
		title: 'Call staff',
		content: 'Do you need the help from our staff?<br>Press "Call staff" below, our staff will go to your table in minutes.',
		buttons: {
			confirm: {
				text: "Call now",
				btnClass: 'btn-orange',
				keys: ['enter'],
				action: function () {

					let tablename = document.getElementById("table_name").innerText

					if (!tablename) {
						showToast("Enter TABLECODE before call staff");
						return;
					}

					if (getCookie("call_staff") == "false") {
						$.alert({
							title: 'Staff',
							icon: 'fa fa-rocket',
							type: 'green',
							content: 'Staff is going to your table.' +
								'<br>' +
								'Please wait and call again after a minute.',
						});
						return;
					}

					CallStaff(tablename);
					setCookie("call_staff", "false");
				}
			},
			cancel: function () { }
		}
	});
})

function CallStaff(tablename) {
	connection.invoke("CallStaff", tablename)

	$.alert({
		title: 'Staff',
		icon: 'fa fa-rocket',
		type: 'green',
		content: 'Staff will go to your table now.' +
			'<br>' +
			'Please wait in minutes.',
	});
}

function setCookie(cname, cvalue) {
	const d = new Date();
	d.setTime(d.getTime() + (60 * 1000));
	let expires = "expires=" + d.toUTCString();
	document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
}

function getCookie(cname) {
	let name = cname + "=";
	let decodedCookie = decodeURIComponent(document.cookie);
	let ca = decodedCookie.split(';');
	for (let i = 0; i < ca.length; i++) {
		let c = ca[i];
		while (c.charAt(0) == ' ') {
			c = c.substring(1);
		}
		if (c.indexOf(name) == 0) {
			return c.substring(name.length, c.length);
		}
	}
	return "";
}

//Call api to update favorite product
let favCheckboxes = document.querySelectorAll(".favcheckbox");

for (let favCheckbox of favCheckboxes) {
	favCheckbox.addEventListener("change", e => {
		let productitem = favCheckbox.closest(".product-item");
		let productId = productitem.getAttribute("data-product-id");
		let favorite = favCheckbox.checked;
		$.ajax({
			url: '/Product/ToggleFavorite',
			type: 'POST',
			data: { productId: productId, favorite: favorite },
			success: function (response) {
				if (response.success) {
					productitem.setAttribute("data-favorite", favorite);
				}
			},
			error: function (xhr, status, error) {
				showToast("Fail to send api")
			},
			async: false
		});
	})
}