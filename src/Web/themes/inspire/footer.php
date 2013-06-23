	<?php if ( woo_active_sidebar('footer-left') || woo_active_sidebar('footer-right' ) ) : ?>
	<div id="footer-widgets">
    	<div class="col-full">

            <div class="left block">
                <?php woo_sidebar('footer-left'); ?>    
            </div>
            <div class="right block">
                <?php woo_sidebar('footer-right'); ?>    
            </div>
            <div class="fix"></div>

		</div><!-- /.col-full  -->
	</div><!-- /#footer-widgets  -->
    <?php endif; ?>
    
	<div id="footer">
    	<div class="col-full">
	
            <div id="copyright" class="col-left">
            <?php if(get_option('woo_footer_left') == 'true'){
            
                    echo stripslashes(get_option('woo_footer_left_text'));	
    
            } else { ?>
                <p>&copy; <?php echo date('Y'); ?> <?php bloginfo(); ?>. <?php _e('All Rights Reserved.', 'woothemes') ?></p>
            <?php } ?>
            </div>
            
            <div id="credit" class="col-right">
            <?php if(get_option('woo_footer_right') == 'true'){
            
                echo stripslashes(get_option('woo_footer_right_text'));
            
            } else { ?>
                <p><?php _e('Powered by', 'woothemes') ?> <a href="http://www.wordpress.org">WordPress</a>. <?php _e('Designed by', 'woothemes') ?> <a href="<?php $aff = get_option('woo_footer_aff_link'); if(!empty($aff)) { echo $aff; } else { echo 'http://www.woothemes.com'; } ?>"><img src="<?php bloginfo('template_directory'); ?>/images/woothemes.png" width="87" height="21" alt="Woo Themes" /></a></p>
            <?php } ?>
            </div>
		
		</div><!-- /.col-full  -->
	</div><!-- /#footer  -->

</div><!-- /#wrapper -->
<?php wp_footer(); ?>
<?php woo_foot(); ?>
</body>
</html>