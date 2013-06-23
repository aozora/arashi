<?php
	global $woo_options;
	query_posts('post_type=slide&order=ASC&orderby=date&post_per_page=20');
	if (have_posts()) : $count = 0; 
?>

<div id="woofader">
    <div id="featured" class="col-full">
    
        <?php if (have_posts()) : while (have_posts()) : the_post(); $count++; ?>		        					
        <div class="slide <?php if($count > 1) echo ' hidden'; ?>">
    
            <?php if ( get_post_meta($post->ID, 'slide_image', true) ) { ?>
            <div class="featured-image">
                <a href="<?php echo get_post_meta($post->ID, "slide_url", $single = true); ?>"><img src="<?php echo get_post_meta($post->ID, "slide_image", $single = true); ?>" alt="" style="margin-bottom:<?php echo $woo_options['woo_featured_image_margin']; ?>px;" /></a>
            </div>
            <?php } elseif ( get_post_meta($post->ID, 'slide_embed', true) ) { ?>  
            	<?php echo woo_embed('key=slide_embed&width=460&height=320&class=video'); ?>
            <?php } ?>
            
            <div class="wrap">
                <?php the_content(); ?>
            </div>
            
        </div><!-- /.slide -->
        <?php endwhile; endif; ?>
        
    </div><!-- /#featured -->
    <div class="fix"></div>
    
	<?php if ($count > 1){ ?>
    <div id="breadcrumb">
        
        <div class="col-full">
            <div class="col">
                <div class="fl">
                    <a href="#" class="left"></a>
                    <a href="#" class="right"></a>
                </div>
                <div class="fr">
                    <ul class="pagination">
                        <?php $count_nav = 1; while ( $count_nav <= $count ) : ?>
                        <li <?php if ( $count_nav == 1 ) echo 'class="active"'; ?>><a href="#"></a></li>
						<?php $count_nav++; endwhile; ?>
                    </ul>
                </div>
                <div class="fix"></div>
            </div><!-- /.col -->
        </div><!-- /.col-full -->
        
    </div><!-- /#breadcrumb -->
	<?php } ?>       
    
</div><!-- /#woofader -->

<?php endif; ?>

<?php 
if ( is_home() && $woo_options['woo_featured_disable'] <> "true" ) { 

if($count > 1){
?>
<script type="text/javascript">
jQuery(document).ready(function(){

	jQuery('#woofader').woofader({
		<?php $speed = $woo_options['woo_featured_speed']; if ( !$speed ) $speed = 500; ?>
		<?php $timeout = $woo_options['woo_featured_timeout']; if ( !$timeout ) $timeout = 0; ?>
		<?php $resize = $woo_options['woo_featured_resize']; if ( !$resize ) $resize = "true"; ?>
		<?php $animate = "true"; if ( $resize <> "true" ) $animate = "false"; ?>
		speed: <?php echo $speed; ?>, 
		timeout: <?php echo $timeout; ?>,
		animate: <?php echo $animate; ?>,
		autoHeight: <?php echo $resize; ?>
	});
	    
});
</script>
<?php } 
} ?>

<?php if(!empty($woo_options['woo_featured_height'])){ ?>
<style type="text/css">
	#featured { height: <?php echo $woo_options['woo_featured_height']; ?>px}
</style>
<?php } ?>
