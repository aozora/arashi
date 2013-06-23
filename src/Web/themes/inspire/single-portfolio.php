<?php get_header(); ?>
<?php global $woo_options; ?>

	<?php woo_crumbs(); ?>
	</div><!-- /#top -->
       
    <div id="content">
	<div class="col-full">   
 
		<div id="main" class="col-left">
		           
            <?php if (have_posts()) : $count = 0; ?>
            <?php while (have_posts()) : the_post(); $count++; ?>
            
				<div id="portfolio" <?php post_class(); ?>>

					<div id="portfolio-image" class="block">
					<?php
                    	// Check if there is a gallery in post
                    	$gallery = woo_get_post_images();
                    	if ( $gallery ) {
                    	
                    		// Get first uploaded image in gallery
                    		$large = $gallery[0]['url'];
                    		$caption = $gallery[0]['caption'];
                    		
	                    } else {
	
							// Grab large portfolio image
						 	$large = get_post_meta($post->ID, 'portfolio-large', $single = true); 
	                    
	                    }

					 	// Setup lightbox if activated
					 	if ( $woo_options['woo_portfolio_lightbox'] == "true" )
					 		$rel = 'rel="prettyPhoto['. $post->ID .']"'
					 ?>
	                    
                    <a <?php echo $rel; ?> title="<?php echo $caption; ?>" href="<?php echo $large; ?>" class="thumb">
						<?php 
						if ( $woo_options['woo_portfolio_resize'] ) {
							woo_image('key=portfolio&width=570&height=272&class=portfolio-img&link=img'); 
						} else { ?>
                        	<img class="portfolio-img" src="<?php echo get_post_meta($post->ID, 'portfolio', $single = true); ?>" alt="" />
                        <?php } ?>
                    </a>
                    
                    <?php 
                    	// Output image gallery for lightbox
                    	if ( $gallery ) {
	                    	foreach ( array_slice($gallery, 1) as $img => $attachment ) {
	                    		echo '<a '.$rel.' title="'.$attachment['caption'].'" href="'.$attachment['url'].'" class="gallery-image"></a>';
	                    	}
	                    }
                    ?>
                    </div>

                    <h1 class="title"><a href="<?php the_permalink() ?>" rel="bookmark" title="<?php the_title(); ?>"><?php the_title(); ?></a></h1>
                    
	        		<?php if(get_post_type() == 'post'){ ?>
                    <p class="post-meta">
                    	<span class="small"><?php _e('by', 'woothemes') ?></span> <span class="post-author"><?php the_author_posts_link(); ?></span>
                    	<span class="small"><?php _e('on', 'woothemes') ?></span> <span class="post-date"><?php the_time(get_option('date_format')); ?></span>
                    	<span class="small"><?php _e('in', 'woothemes') ?></span> <span class="post-category"><?php the_category(', ') ?></span>
   	                    <?php edit_post_link( __('{ Edit }', 'woothemes'), '<span class="small">', '</span>' ); ?>
                    </p>
                    <?php } ?>
                    
                    <div class="entry">
                    	<?php the_content(); ?>
					</div>
										
					<?php the_tags('<p class="tags">Tags: ', ', ', '</p>'); ?>

                    <?php // woo_postnav(); ?>
                    
                </div><!-- /.post -->
                
                <?php $comm = get_option('woo_comments'); if ( 'open' == $post->comment_status && ($comm == "post" || $comm == "both") ) : ?>
	                <?php comments_template('', true); ?>
                <?php endif; ?>
                                                    
			<?php endwhile; else: ?>
				<div class="post">
                	<p><?php _e('Sorry, no posts matched your criteria.', 'woothemes') ?></p>
  				</div><!-- /.post -->             
           	<?php endif; ?>  
        
		</div><!-- /#main -->

        <?php get_sidebar(); ?>

	</div><!-- /#col-full -->
    </div><!-- /#content -->
		
<?php get_footer(); ?>