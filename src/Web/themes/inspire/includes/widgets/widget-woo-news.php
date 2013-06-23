<?php
// =============================== News from the blog widget ======================================

class Woo_News extends WP_Widget {

   function Woo_News() {
	   $widget_ops = array('description' => 'Show Latest News widget' );
       parent::WP_Widget(false, __('Woo - News', 'woothemes'),$widget_ops);      
   }

   function widget($args, $instance) {  
    extract( $args );
   	$title = $instance['title']; if (!$title) $title = 'News from the blog';
   	$number = $instance['number'];
	$size = $instance['size']; if (!$size && size <> 0) $size = 70;
	$align = $instance['align']; if (!$align) $align = 'alignright';
	?>
		<?php echo $before_widget; ?>

				<h3><?php echo $title; ?></h3>
                <a class="rss" href="<?php if ( get_option('woo_feed_url') <> "" ) { echo get_option('woo_feed_url'); } else { echo bloginfo('url') . "/?feed=rss2"; } ?>" title="<?php _e('Subscribe to our RSS feed', 'woothemes'); ?>"><img src="<?php bloginfo('template_directory'); ?>/images/ico-rss-big.png" alt="RSS"/></a>
                
				
				<div>
					<?php
						 query_posts('posts_per_page=' . $number);
						 if ( have_posts() ) : while ( have_posts() ) : the_post();
					?>
					    <div class="item">
                        	<?php if ( $size <> 0 ) woo_image('width='.$size.'&height='.$size.'&class=thumbnail '.$align); ?> 
                        	<a class="title" href="<?php echo get_permalink($post->ID); ?>" title="<?php echo get_the_title($post->ID); ?>"><?php echo get_the_title($post->ID); ?></a>
                            <p class="post-meta">
                                <span class="small"><?php _e('by', 'woothemes') ?></span> <span class="post-author"><?php the_author_posts_link(); ?></span>
                                <span class="small"><?php _e('on', 'woothemes') ?></span> <span class="post-date"><?php the_time(get_option('date_format')); ?></span>
                                <span class="small"><?php _e('in', 'woothemes') ?></span> <span class="post-category"><?php the_category(', ') ?></span>
                            </p>
                            <p><?php echo woo_text_trim( get_the_excerpt(), 25); ?></p>
                        </div>
					    
					<?php endwhile; endif; ?>
				</div>
			
		<?php echo $after_widget; ?>   
   <?php
   }

   function update($new_instance, $old_instance) {                
       return $new_instance;
   }

   function form($instance) {        
   
       $title = esc_attr($instance['title']);
       $number = esc_attr($instance['number']);
       $size = esc_attr($instance['size']);
       $align = esc_attr($instance['align']);
       ?>
       <p>
	   	   <label for="<?php echo $this->get_field_id('title'); ?>"><?php _e('Title:','woothemes'); ?></label>
	       <input type="text" name="<?php echo $this->get_field_name('title'); ?>"  value="<?php echo $title; ?>" class="widefat" id="<?php echo $this->get_field_id('title'); ?>" />
       </p>
       <p>
	   	   <label for="<?php echo $this->get_field_id('number'); ?>"><?php _e('Number:','woothemes'); ?></label>
	       <input type="text" name="<?php echo $this->get_field_name('number'); ?>"  value="<?php echo $number; ?>" class="widefat" id="<?php echo $this->get_field_id('number'); ?>" />
       </p>
       <p>
	   	   <label for="<?php echo $this->get_field_id('size'); ?>"><?php _e('Thumbnail Size (0 disable):','woothemes'); ?></label>
	       <input type="text" name="<?php echo $this->get_field_name('size'); ?>"  value="<?php echo $size; ?>" class="widefat" id="<?php echo $this->get_field_id('size'); ?>" />
       </p>
        <p>
            <label for="<?php echo $this->get_field_id('align'); ?>"><?php _e('Thumb Align:','woothemes'); ?></label>
            <select name="<?php echo $this->get_field_name('align'); ?>" class="widefat" id="<?php echo $this->get_field_id('align'); ?>">
                <option value="alignleft" <?php if($align == "alignleft"){ echo "selected='selected'";} ?>><?php _e('Left', 'woothemes'); ?></option>
                <option value="alignright" <?php if($align == "alignright"){ echo "selected='selected'";} ?>><?php _e('Right', 'woothemes'); ?></option>            
            </select>
        </p>
        <p>
      <?php
   }
} 

register_widget('Woo_News');
