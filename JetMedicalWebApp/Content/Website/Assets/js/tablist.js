(function($) {
	'use strict';

	// Tilføjer plugin til fn så det kan bruges normalt
	$.fn.monica = function (options) {

		// Options
		// Sætter default options, hvis der ikke sendes noget med
		const defaultOptions = {

			settings: {
				upTime: "fast", // slower, slow, fast, faster
				downTime: "fast" // slower, slow, fast, faster
			}
		
		};
		
		let opts = $.extend(true, {}, defaultOptions, options);

		// Går gennem alle elementer der matcher selector = idx=index, el=element
		return this.each(function (idx, el) {

			$(".monicaTitle").click(function () {

				$(".moniceContent").slideUp(opts.settings.upTime);
				$(this).next("div").slideDown(opts.settings.downTime);

			})

		});
	};

})(jQuery);