<?php 

/*-----------------------------------------------------------------------------------

TABLE OF CONTENTS

- Custom theme actions/functions
	- Add specific IE styling/hacks to HEAD
	- Add custom styling
- Custom hook definitions
- Set portfolio ID

-----------------------------------------------------------------------------------*/

/*-----------------------------------------------------------------------------------*/
/* Custom functions */
/*-----------------------------------------------------------------------------------*/

// Add specific IE styling/hacks to HEAD
add_action('wp_head','woo_IE_head');
function woo_IE_head() {
?>

<!--[if IE 6]>
<script type="text/javascript" src="<?php bloginfo('template_directory'); ?>/includes/js/pngfix.js"></script>
<script type="text/javascript" src="<?php bloginfo('template_directory'); ?>/includes/js/menu.js"></script>
<link rel="stylesheet" type="text/css" media="all" href="<?php bloginfo('template_directory'); ?>/css/ie6.css" />
<![endif]-->	

<!--[if IE 7]>
<link rel="stylesheet" type="text/css" media="all" href="<?php bloginfo('template_directory'); ?>/css/ie7.css" />
<![endif]-->

<!--[if IE 8]>
<link rel="stylesheet" type="text/css" media="all" href="<?php bloginfo('template_directory'); ?>/css/ie8.css" />
<![endif]-->

<?php
}

// Add custom styling
add_action('woo_head','woo_custom_styling');
function woo_custom_styling() {
	
	// Get options
	$body_color = get_option('woo_body_color');
	$body_img = get_option('woo_body_img');
	$body_repeat = get_option('woo_body_repeat');
	$body_position = get_option('woo_body_pos');
	$link = get_option('woo_link_color');
	$hover = get_option('woo_link_hover_color');
	$button = get_option('woo_button_color');
		
	// Add CSS to output
	if ($body_color)
		$output .= '#top {background-color:'.$body_color.';background-image:none}' . "\n";
	if ($body_img)
		$output .= '#top {background-image:url('.$body_img.')}' . "\n";
	if ($body_repeat && $body_position)
		$output .= '#top {background-repeat:'.$body_repeat.'}' . "\n";
	if ($body_img && $body_position)
		$output .= '#top {background-position:'.$body_position.'}' . "\n";
	if ($link)
		$output .= 'a:link, a:visited {color:'.$link.'}' . "\n";
	if ($hover)
		$output .= 'a:hover {color:'.$hover.'}' . "\n";
	if ($button)
		$output .= '.button, .reply a {background-color:'.$button.' !important}' . "\n";
	
	// Output styles
	if (isset($output)) {
		$output = "<!-- Woo Custom Styling -->\n<style type=\"text/css\">\n" . $output . "</style>\n";
		echo $output;
	}
		
} 



/*-----------------------------------------------------------------------------------*/
/* Custom Hook definition */
/*-----------------------------------------------------------------------------------*/

// Add any custom hook definitions you want here
// function woo_hook_name() { do_action( 'woo_hook_name' ); }					



?>