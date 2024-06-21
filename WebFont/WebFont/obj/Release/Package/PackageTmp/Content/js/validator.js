
const Validator = function(options){
    const formData = document.getElementById(options.form);
    const selectorRules = [];   // Save an array of validation requests for each field
    let isValid;

    // Continue if form element exists
    if (formData){
        // Loop all validation requests
        options.rules.forEach((rule) => {

            // Push into if the validation requests array of this field already exists
            if (Array.isArray(selectorRules[rule.selector])){
                selectorRules[rule.selector].push(rule.functionTest);
            } else {
                // Add a new validation requests array of this field
                selectorRules[rule.selector] = [rule.functionTest];
            }

            let input = formData.querySelector(rule.selector);

            if (input){
                // validate data of input field when user click out
                input.addEventListener('blur', () => {
                    validate(input, rule);
                });

                // Remove error message of input field when user is typing
                input.addEventListener('input', () => {
                    const inputParentElement = getParentElement(input, options.formInput);
                    const errorMessage = inputParentElement.querySelector(options.errorMessage);
                    
                    errorMessage.innerText = '';
                    inputParentElement.classList.remove('invalid');
                });
            }
        });
    }

    // Get parent element of input field
    function getParentElement(input, fromInput){
        let parentInput = input.parentElement;

        while(parentInput){
            if (parentInput.matches(fromInput)){
                return parentInput;
            }
            input = input.parentElement;
            parentInput = input.parentElement;
        }
    }

    // Perform all validation requests of this input field (e.g. isRequired, isEmail, minLenght,...)
    function validate(input, rule){
        const inputParentElement = getParentElement(input, options.formInput);
        const errorMessageElement = inputParentElement.querySelector(options.errorMessage);

        let rules = selectorRules[rule.selector];   // Get the validation requests array of this field by selector
        let messageError;

        for (let i=0; i < rules.length; ++i){
            messageError = rules[i](input.value.trim());    // Call validation function (e.g. isRequired, isEmail,...)

            if(messageError)
                break;
        }

        if (messageError){
            errorMessageElement.innerText = messageError;
            inputParentElement.classList.add('invalid');
        } else {
            errorMessageElement.innerText = '';
            inputParentElement.classList.remove('invalid');
        }

        return !!messageError; // Convert string to boolean
    }

    formData.addEventListener('submit', function(e){
        e.preventDefault();     // Stop default action when submit

        let isFormValid = false;

        // Loop all validation requests of form
        options.rules.forEach((rule) => {
            let input = formData.querySelector(rule.selector);

            isValid = validate(input, rule);

            if (isValid){
                isFormValid = true;
            }
        });

        if (!isFormValid){
            // Submit by custom method
            if (typeof options.onSubmit === 'function'){
                let inputs = formData.querySelectorAll('[name]');

                // Get values from inputs to object
                let formValues = Array.from(inputs).reduce((values, input) => {
                    values[input.name] = input.value.trim();

                    return values;
                }, {});
                
                options.onSubmit(formValues);
            } else {
                // Submit by default method
                formData.submit();
            }
        }
    });
}

// Check field is required
Validator.isRequired = function(selector, message){
    return {
        selector: selector,
        functionTest: function(value){
            return value ? undefined : message || 'Trường này là bắt buộc!';
        }
    }
}

// Check email format
Validator.isEmail = function(selector, message){
    return {
        selector: selector,
        functionTest: function(value){
            var re = /^(([^<>()\[\]\.,;:\s@\"]+(\.[^<>()\[\]\.,;:\s@\"]+)*)|(\".+\"))@(([^<>()[\]\.,;:\s@\"]+\.)+[^<>()[\]\.,;:\s@\"]{2,})$/i;
            return re.test(value) ? undefined : message || 'Trường này là bắt buộc!';
        }
    }
}

// Check minimum lenght of field (password, phone)
Validator.minLength = function(selector, minLength, message){
    return {
        selector: selector,
        functionTest: function(value){
            return value.length >= minLength ? undefined : message || `Vui lòng nhập tối thiểu ${minLength} kí tự!`;
        }
    }
}

// Check value of repeat field
Validator.isConfirmed = function(selector, confirmSelector, message){
    return {
        selector: selector,
        functionTest: function(value){
            // Get value of field needs confirmation
            let confirmValue = document.querySelector(confirmSelector).value.trim();

            return value == confirmValue ? undefined : message || 'Giá trị nhập lại không đúng!';
        }
    }
}