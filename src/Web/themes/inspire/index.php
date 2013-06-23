<?php get_header(); ?>
<?php global $woo_options; ?>

	<?php if ( $woo_options['woo_featured_disable'] <> "true" ) include( TEMPLATEPATH . '/includes/featured.php'); ?>
	
	</div><!-- /#top -->


    <div id="content">
	<div class="col-full">   
     
		<div id="main" class="col-left">     
        
	        <?php if ( $woo_options['woo_main_page1'] && $woo_options['woo_main_page1'] <> "Select a page:" ) { ?>
	        <div id="main-page1">
				<?php query_posts('page_id=' . get_page_id($woo_options['woo_main_page1'])); ?>
	            <?php if (have_posts()) : while (have_posts()) : the_post(); ?>		        					
			    <div class="entry"><?php the_content(); ?></div>
	            <?php endwhile; endif; ?>
	            <div class="fix"></div>
	        </div><!-- /#main-page1 -->
	        <?php } ?>
            
	        <?php if ( $woo_options['woo_main_pages'] == 'true' ) { ?>
            <div id="mini-features">
		        <?php query_posts('post_type=infobox&order=ASC&posts_per_page=20'); ?>
		        <?php if (have_posts()) : while (have_posts()) : the_post(); $counter++; ?>		        					
    
                <div class="block<?php if ( $counter == 2 ) echo ' last'; ?>">
                    <?php if ( get_post_meta($post->ID, 'mini', true) ) { ?>
                    <img src="<?php echo get_post_meta($post->ID, 'mini', $single = true); ?>" alt="" class="home-icon" />				
                    <?php } ?> 
                                                         
                    <div class="<?php if ( get_post_meta($post->ID, 'mini', true) ) echo 'feature'; ?>">
                       <h3><?php echo get_the_title(); ?></h3>
                       <p><?php echo get_post_meta($post->ID, 'mini_excerpt', true) ?></p>
                       <?php if ( get_post_meta($post->ID, 'mini_readmore', true) ) { ?><a href="<?php echo get_post_meta($post->ID, 'mini_readmore', $single = true); ?>" class="btn"><span><?php _e('Read More', 'woothemes'); ?></span></a><?php } ?>
                    </div>
                </div>
                <?php if ( $counter == 2 ) { $counter = 0; echo '<div class="fix"></div>'; } ?>				
                    
            <?php endwhile; endif; ?>
                
                <div class="fix"></div>
                
                <?php if ( $woo_options['woo_main_pages_link'] ) { ?>
                <div class="more-features">
                	<a href="<?php echo $woo_options['woo_main_pages_link']; ?>"><span><?php echo $woo_options['woo_main_pages_link_text']; ?></span><img src="<?php bloginfo('template_directory'); ?>/images/btn-more.png" alt="" /></a>
                </div>
                <?php } ?>
            </div><!-- /#mini-features -->
            <?php } ?>
                    
            <div class="fix"></div>
                        
	        <?php if ( $woo_options['woo_main_page2'] && $woo_options['woo_main_page2'] <> "Select a page:" ) { ?>
	        <div id="main-page2">
				<?php query_posts('page_id=' . get_page_id($woo_options['woo_main_page2'])); ?>
	            <?php if (have_posts()) : while (have_posts()) : the_post(); ?>		        					
			    <div class="entry"><?php the_content(); ?></div>
	            <?php endwhile; endif; ?>
	            <div class="fix"></div>
	        </div><!-- /#main-page2 -->
	        <?php } ?>
               
                
		</div><!-- /#main -->

		<?php get_sidebar(); ?>		           

	</div><!-- /#col-full -->
    </div><!-- /#content -->
		
<?php get_footer(); ?>