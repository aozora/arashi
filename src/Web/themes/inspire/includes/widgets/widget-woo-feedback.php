<?php
/*---------------------------------------------------------------------------------*/
/* Feedback widget */
/*---------------------------------------------------------------------------------*/
class Woo_Feedback extends WP_Widget {

   function Woo_Feedback() {
	   $widget_ops = array('description' => 'Add customer feedback.' );
       parent::WP_Widget(false, __('Woo - Feedback', 'woothemes'),$widget_ops);      
   }
   
   function widget($args, $instance) {  
    extract( $args );
   	$title = $instance['title']; if (!$title) $title = __( "Feedback", 'woothemes' );
	$unique_id = $args['widget_id'];
	?>

        <div id="<?php echo $unique_id; ?>" class="widget_woo_feedback widget">
        
            <?php if ($title) { ?><h3><?php echo $title; ?></h3><?php } ?>
        
			<?php query_posts('post_type=feedback&orderby=rand'); ?>
            <?php if (have_posts()) : ?>
            <div class="feedback">
            	<div class="quotes">
            
	   			<?php while (have_posts()) : the_post(); ?>		      
	            	<div class="quote">
	                    <blockquote><?php the_content(); ?></blockquote>
	                    <cite><?php echo get_post_meta(get_the_ID(),'feedback_citation',true); ?></cite>
	                </div>
	            <?php endwhile; ?>

	        	</div>
        	</div>
            
            <?php endif; ?>

        </div>
   		
	<?php
   }

   function update($new_instance, $old_instance) {                
       return $new_instance;
   }

   function form($instance) {        
   
       $title = esc_attr($instance['title']);
       $text = esc_attr($instance['text']);
	   $citation = esc_attr($instance['citation']);
       ?>
       <p>Use the Feedback custom post type to add content to this widget.</p>
       <p>
	   	   <label for="<?php echo $this->get_field_id('title'); ?>"><?php _e('Title:','woothemes'); ?></label>
	       <input type="text" name="<?php echo $this->get_field_name('title'); ?>"  value="<?php echo $title; ?>" class="widefat" id="<?php echo $this->get_field_id('title'); ?>" />
       </p>
      <?php
   }
   
} 
register_widget('Woo_Feedback');

// Add Javascript
if(is_active_widget( null,null,'woo_feedback' ) == true && !is_admin()) {
	add_action('wp_print_scripts','woo_widget_feedback_js');
	add_action('wp_footer','woo_widget_feedback_js_output');
}

function woo_widget_feedback_js(){
	wp_enqueue_script( 'innerfade', get_template_directory_uri().'/includes/js/innerfade.js', array( 'jquery' ) );
}

function woo_widget_feedback_js_output() {
// feedback widget
?>
<script type="text/javascript">
jQuery(document).ready( function(){ 
    jQuery('.quotes').innerfade({
        animationtype: 'fade', 
        speed: 'normal', 
        timeout: 6000, 
        type: 'random_start'
    });
    
});
</script>
<?php 
}
