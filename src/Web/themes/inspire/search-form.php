<div id="search_main" class="widget">
    <form method="get" id="searchform" action="<?php bloginfo('url'); ?>">
        <input type="text" class="field" name="s" id="s"  value="<?php _e('Enter search keywords...', 'woothemes') ?>" onfocus="if (this.value == '<?php _e('Enter search keywords...', 'woothemes') ?>') {this.value = '';}" onblur="if (this.value == '') {this.value = '<?php _e('Enter search keywords...', 'woothemes') ?>';}" />
        <input name="Submit" type="image" src="<?php bloginfo('template_directory'); ?>/images/ico-search.png" value="Go" class="btn"  />
    </form>
</div>
