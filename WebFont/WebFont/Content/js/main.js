const popup = document.getElementById('popup');
const popupContent = document.getElementById('popup-content');
const popupCloseButton = document.getElementById('popup-close-button');
const userControls = document.getElementById('user-controls');
const userButton = document.getElementById('user-button');
const userNav = document.getElementById('user-nav');
const logoutButton = document.getElementById('logout');
const loadingAnimation = document.getElementById('popup-loader');

const formSignInHtml = `
    <div id="form-signin" class="form">
        <h2>Đăng nhập</h2>

        <form id="form-login" action="#" class="mt-4">
            <div class="input-box">
                <span class="label-error"></span>
                <input type="text" placeholder="Nhập username" id="username" name="username">
            </div>
            <div class="input-box">
                <span class="label-error"></span>
                <input type="password" placeholder="Nhập mật khẩu" id="password" name="password">
            </div>
            <div class="input-box button">
                <button type="submit">Đăng nhập</button>
            </div>
        </form>

        <h3 class="change-link m-0 pt-3 w-100 text-center">Bạn chưa có tài khoản? <span id="register-link" class="link ms-1">Đăng ký ngay</span></h3>
        <h3 class="change-link m-0 pt-2 w-100 text-center"><a href="" class="link">Quên mật khẩu</a></h3>
    </div>
`;

const formSignUpHtml = `
    <div id="form-signup" class="form">
        <h2>Đăng ký</h2>

        <form id="form-register" action="#" class="mt-4">
            <div class="input-box">
                <span class="label-error"></span>
                <input type="text" placeholder="Nhập username" id="username" name="username">
            </div>
            <div class="input-box">
                <span class="label-error"></span>
                <input type="text" placeholder="Nhập email của bạn" id="email" name="email">
            </div>
            <div class="input-box">
                <span class="label-error"></span>
                <input type="password" placeholder="Nhập mật khẩu của bạn" id="password" name="password">
            </div>
            <div class="input-box">
                <span class="label-error"></span>
                <input type="password" placeholder="Xác nhận lại mật khẩu" id="repeat-password" name="repeat-password">
            </div>
            <div class="policy">
                <input type="checkbox" aria-label="checkbox" name="policy" required>
                <h3 class="mb-0">Tôi đồng ý với mọi <a href="#">điều khoản và điều kiện</a></h3>
            </div>
            <div class="input-box button">
                <button type="submit">Đăng ký ngay</button>
            </div>
        </form>

        <h3 class="change-link m-0 pt-3 w-100 text-center">Bạn đã có tài khoản? <span id="login-link" class="link">Đăng nhập</span></h3>
    </div>
`;

function openLoadingAnimation(){
    loadingAnimation.style.display = 'block';
    
    setTimeout(function(){
		closeLoadingAnimation();
	}, 1500);
}

function closeLoadingAnimation(){
    loadingAnimation.style.display = 'none';
}

function openPopup(content){
    popupContent.innerHTML = content;
    popup.style.display = 'block';
    
    closePopupListener();
}

function closePopup(){
    popupContent.innerHTML = '';
    popup.style.display = 'none';
}

function closePopupListener(){
    popupCloseButton.addEventListener('click', function(){
        closePopup();
    });
}

function openUserNav(){
    if (userNav.classList.contains('active')){
        userNav.classList.remove('active');
    } else {
        userNav.classList.add('active');
    }
}

function changeToRegister(){
    openPopup(formSignUpHtml);
    const loginLink = document.getElementById('login-link');

    loginLink.addEventListener('click', function(){
        changeToLogin();
    });

    validateRegister();
}

function changeToLogin(){
    openPopup(formSignInHtml);
    const registerLink = document.getElementById('register-link');

    registerLink.addEventListener('click', function(){
        changeToRegister();
    });

    validateLogin();
}

function loginViews(){
    closePopup();
    userControls.classList.add('login');
}

function logoutViews(){
    userControls.classList.remove('login');
}

function setLoginState(){
	localStorage.setItem('authenticationState', true);
}

function setLogoutState(){
	localStorage.removeItem('jwtToken');
	localStorage.setItem('authenticationState', false);
}

function getAuthenticationSate(){
	let status = localStorage.getItem('authenticationState');
	
	return status === "true";
}

function setViewsByAuthenticationState(){
	let status = getAuthenticationSate();

	if (status == true){
        loginViews();
       
        userButton.onclick = function(){
            openUserNav();    // Permit action open user nav controls if user has been authenticated
        };

        logoutButton.onclick = function(){
            logout();       // Permit action logout if user has been authenticated
        };
    } else {
        logoutViews();

        userButton.onclick = function(){
            changeToLogin();    // Permit action login if user was not authenticated
        };      
    }
}