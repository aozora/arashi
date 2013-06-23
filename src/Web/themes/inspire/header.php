<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head profile="http://gmpg.org/xfn/11">

<title><?php woo_title(); ?></title>
<?php woo_meta(); ?>

<link rel="stylesheet" type="text/css" href="<?php bloginfo('stylesheet_url'); ?>" media="screen" />
<link rel="stylesheet" type="text/css" media="all" href="<?php bloginfo('template_directory'); ?>/css/effects.css" />
<link rel="stylesheet" type="text/css" media="all" href="<?php bloginfo('template_directory'); ?>/css/prettyPhoto.css" />
<link rel="alternate" type="application/rss+xml" title="RSS 2.0" href="
<?php $feedurl = get_option('woo_feed_url'); 
if ( !empty($feedurl) ) { 
	echo $feedurl; 
} else { 
	echo bloginfo('url') . "/?feed=rss2";
} 
?>
" />
<link rel="pingback" href="<?php bloginfo('pingback_url'); ?>" />
<?php if ( get_option('woo_google_fonts') == "true" ) { ?>
<link href='http://fonts.googleapis.com/css?family=Droid+Sans|Droid+Serif:italic' rel='stylesheet' type='text/css' />
<?php } ?>      

<?php if ( is_singular() ) wp_enqueue_script( 'comment-reply' );  ?>
<?php wp_head(); ?>
<?php woo_head(); ?>

<!--[if lte IE 7]>
<script type="text/javascript">
jQuery(function() {
	var zIndexNumber = 1000;
	jQuery('div').each(function() {
		jQuery(this).css('zIndex', zIndexNumber);
		zIndexNumber -= 10;
	});
});
</script>
<![endif]-->

</head>

<body <?php body_class(); ?>>

<?php woo_top(); ?>

<div id="wrapper">

	<div id="top">
           
        <div id="header">
        <div class="col-full">
       
            <div id="logo" class="fl">
               
                <?php if (get_option('woo_texttitle') <> "true") { ?><a href="<?php bloginfo('url'); ?>" title="<?php bloginfo('description'); ?>"><img class="title" src="<?php if ( get_option('woo_logo') <> "" ) { echo get_option('woo_logo'); } else { bloginfo('template_directory'); ?>/images/logo.png<?php } ?>" alt="<?php bloginfo('name'); ?>" /></a><?php } ?>
                
                <?php if(is_single() || is_page()) : ?>
                    <span class="site-title"><a href="<?php bloginfo('url'); ?>"><?php bloginfo('name'); ?></a></span>
                <?php else: ?>
                    <h1 class="site-title"><a href="<?php bloginfo('url'); ?>"><?php bloginfo('name'); ?></a></h1>
                <?php endif; ?>
                
                    <span class="site-description"><?php bloginfo('description'); ?></span>
                
            </div><!-- /#logo -->
            <?php
			if ( function_exists('has_nav_menu') && has_nav_menu('primary-menu') ) {
				wp_nav_menu( array( 'depth' => 4, 'sort_column' => 'menu_order', 'container' => 'ul', 'menu_id' => 'nav', 'menu_class' => 'fr', 'theme_location' => 'primary-menu' ) );
			} else {
			?>   
            <ul id="nav" class="fr">
            
			<?php 
        	if ( get_option('woo_custom_nav_menu') == 'true' ) {
        		if ( function_exists('woo_custom_navigation_output') )
					woo_custom_navigation_output('depth=4');

			} else { ?>

                <?php if (is_page() || is_archive() || is_single() || is_tag()) { $highlight = "page_item"; } else {$highlight = "page_item current_page_item"; } ?>
                <li class="<?php echo $highlight; ?> home_link"><a href="<?php bloginfo('url'); ?>"><?php _e('Home', 'woothemes') ?></a></li>
                <?php  wp_list_pages('sort_column=menu_order&depth=4&title_li='); ?>
                
			<?php } ?>
                
            </ul><!-- /#nav -->
            <?php } ?>               
        </div><!-- /.col-full -->
        </div><!-- /#header -->