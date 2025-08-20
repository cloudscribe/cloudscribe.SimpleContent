// Unsaved changes warning for Summernote editor
(function() {
    'use strict';
    
    let summernoteChanged = false;
    
    // Wait for DOM to be ready
    document.addEventListener('DOMContentLoaded', function() {
        // Find all Summernote textareas
        const summernoteTextareas = document.querySelectorAll('[data-summernote-unobtrusive]');
        if (summernoteTextareas.length === 0) {
            return; // No Summernote on this page
        }
        
        // Wait for Summernote to initialize (it's loaded asynchronously)
        const checkSummernote = setInterval(function() {
            if (typeof $ !== 'undefined' && $.fn.summernote) {
                // Check if all Summernote instances are initialized
                let allInitialized = true;
                summernoteTextareas.forEach(function(textarea) {
                    if (!$(textarea).data('summernote')) {
                        allInitialized = false;
                    }
                });
                
                if (allInitialized) {
                    clearInterval(checkSummernote);
                    
                    // Listen for Summernote change events on all instances
                    summernoteTextareas.forEach(function(textarea) {
                        $(textarea).on('summernote.change', function() {
                            summernoteChanged = true;
                        });
                    });
                    
                    // Find Developer Tools link
                    const devToolsLink = document.querySelector('a[data-unsaved-warning]');
                    if (devToolsLink) {
                        devToolsLink.addEventListener('click', function(e) {
                            if (summernoteChanged) {
                                const warningText = devToolsLink.getAttribute('data-unsaved-warning') || 'You have unsaved changes in the editor. Are you sure you want to navigate to Developer Tools? Your changes will be lost.';
                                const confirmLeave = confirm(warningText);
                                if (!confirmLeave) {
                                    e.preventDefault();
                                    return false;
                                }
                            }
                        });
                    }
                    
                    // Reset the flag when form is submitted successfully
                    const form = document.querySelector('form[data-submit-once="true"]');
                    if (form) {
                        form.addEventListener('submit', function() {
                            summernoteChanged = false;
                        });
                    }
                }
            }
        }, 100);
        
        // Stop checking after 10 seconds (failsafe)
        setTimeout(function() {
            clearInterval(checkSummernote);
        }, 10000);
    });
})();