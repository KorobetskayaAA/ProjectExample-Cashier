export function setSubmitting(form, submitting) {
    form.submitting = submitting;
    const submitButton = form.querySelector('button[type="submit"]');
    submitButton.submitting = submitting;
}

export function setAlert(form, message, color = "danger") {
    if (!form.alertElement) {
        form.alertElement = document.createElement("alert-message");
        form.append(form.alertElement);
    }
    form.alertElement.color = color;
    form.alertElement.innerText = message;
    form.alertElement.hidden = !message || undefined;
}

export function attributeValueToBool(value) {
    return value === "" || (!!value && value !== "false");
}

export function attributeValueToFunction(value) {
    if (value instanceof Function) {
        return value;
    }
    const evaluation = eval(value);
    if (evaluation instanceof Function) {
        return evaluation;
    }
    return () => evaluation;
}
