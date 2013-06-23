<?php
/*
Template Name: Portfolio
*/
?>

<?php global $woo_options; ?>

<?php get_header(); ?>

	<!-- Breadcrumb & Tags -->
	<?php if ( $woo_options['woo_portfolio_tags'] ) { ?>
		<div id="breadcrumb">
        	<div class="col-full">
                <div class="fl">
                	<?php
					$tags = explode(',',$woo_options['woo_portfolio_tags']); // Tags to be shown
					foreach ($tags as $tag){
						$tag = trim($tag); 
						$displaytag = $tag;
						$tag = str_replace (" ", "-", $tag);	
						$tag = str_replace ("/", "-", $tag);
						$tag = strtolower ( $tag );
						$link_tags[] = '<a href="#" rel="'.$tag.'">'.$displaytag.'</a>'; 
					}
					$new_tags = implode(' ',$link_tags);
					?>
                    <span class="port-cat"><?php _e('Select a category:', 'woothemes'); ?>&nbsp;&nbsp;&nbsp;&nbsp;<a href="#" rel="all"><?php _e('All','woothemes'); ?></a>&nbsp;<?php echo $new_tags; ?></span>
                </div>
                <a class="subscribe fr" href="<?php if ( $woo_options['woo_feed_url'] <> "" ) { echo $woo_options['woo_feed_url']; } else { echo bloginfo('url') . "/?feed=rss2"; } ?>">
                    <img src="<?php bloginfo('template_directory'); ?>/images/ico-rss.png" alt="Subscribe" class="rss" />
                </a>        
                <div class="<?php if ( $bc == "true" ) echo 'fr'; else echo 'fl'; ?>">
					<span class="fr catrss"><a class="subscribe fr" href="<?php if ( $woo_options['woo_feed_url'] <> "" ) { echo $woo_options['woo_feed_url']; } else { echo get_bloginfo_rss('rss2_url'); } ?>"></a></span>
                </div>
			</div>
        </div>   
	<?php } else { ?>
		<?php woo_crumbs(); ?>   
	<?php } ?>
	<!-- /Breadcrumb & Tags -->
    
	</div><!-- /#top -->
       
    <div id="content">
    <div class="col-full">
    
		<div id="portfolio">
            			
	        <?php $paged = get_query_var('paged'); query_posts("post_type=portfolio&posts_per_page=-1&paged=$paged"); ?>
	        <?php if (have_posts()) : while (have_posts()) : the_post(); $counter++; ?>		        					

				<?php 
					// Portfolio tags class
					$porttag = ""; 
					$posttags = get_the_tags(); 
					if ($posttags) { 
						foreach($posttags as $tag) { 
							$tag = $tag->name;
							$tag = str_replace (" ", "-", $tag);	
							$tag = str_replace ("/", "-", $tag);
							$tag = strtolower ( $tag );
							$porttag .= $tag . ' '; 
						} 
					} 
				?>                                                                        
                <!-- Post Starts -->
                <div class="post block fl <?php echo $porttag; ?>">

					<?php if ( woo_image('key=portfolio&return=true') ) : 
						// Grab large portfolio image
					 	$large = get_post_meta($post->ID, 'portfolio-large', $single = true); 

						if (empty($large) && $woo_options['woo_portfolio_lightbox'] == "true") { 
	                    	// Check if there is a gallery in post
	                    	$exclude = get_post_meta($post->ID, 'portfolio', $single = true);
	                    	$gallery = woo_get_post_images();
	                    	if ( $gallery ) {
	                    		// Get first uploaded image in gallery
	                    		$large = $gallery[0]['url'];
	                    		$caption = $gallery[0]['caption'];
		                    } 
	                    }
	                    
	                    // Set rel on anchor to show lightbox
	                    if ( $woo_options['woo_portfolio_lightbox'] == "true") 
                    		$rel = 'rel="prettyPhoto['. $post->ID .']"';
					 ?>
	                    
                    <a <?php echo $rel; ?> title="<?php echo $caption; ?>" href="<?php echo $large; ?>" class="thumb">
						<?php 
						if ( $woo_options['woo_portfolio_resize'] ) {
							woo_image('key=portfolio&width=440&height=210&class=portfolio-img&link=img'); 
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
	                    	unset($gallery);
	                    }
                    endif; ?>

                    <h2 class="title"><?php the_title(); ?></h2>

                    <div class="entry">
	                    <?php the_content(); ?>                        
	                </div><!-- /.entry -->

                </div><!-- /.post -->
                                                    
			<?php endwhile; else: ?>
				<div class="post">
				     <p class="note">You need to setup the <strong>Portfolio</strong> options and select a category for your portfolio posts.</p>
                </div><!-- /.post -->
            <?php endif; ?>  

            <div class="fix"></div>
        
            <?php woo_pagenav(); ?>
                
		</div><!-- /#portfolio -->
        

    </div><!-- /.col-full -->
    </div><!-- /#content -->    
        
<?php get_footer(); ?>