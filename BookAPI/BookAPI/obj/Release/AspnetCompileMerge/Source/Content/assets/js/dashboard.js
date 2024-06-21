const loadingAnimation = document.getElementById('popup-loader');

function openLoadingAnimation(){
    loadingAnimation.style.display = 'block';
    
    setTimeout(function(){
		closeLoadingAnimation();
	}, 1500);
}

function closeLoadingAnimation(){
    loadingAnimation.style.display = 'none';
}
