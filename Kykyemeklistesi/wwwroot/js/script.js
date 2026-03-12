$(document).ready(function () {
    console.log("deneme");


    $("#excel").on("click", function () {
        $(this).append('<span class="loading-text ms-2">İndiriliyor...</span>');
        setTimeout(() => {
            $(".loading-text").fadeOut(500, function () { $(this).remove(); });
        }, 3000);
    });

    $("#excel").on("click", function () {
        $(this).addClass("shake");

        setTimeout(() => {
            $(this).removeClass("shake");
        }, 500);
    });



});

function oyver(yemekId, isLike, element) {
    const votingSection = element.closest('.voting-section');
    const buttons = votingSection.querySelectorAll('.btn-vote');
    
    // Butonları geçici olarak devre dışı bırak
    buttons.forEach(btn => btn.disabled = true);

    fetch('/Home/Oyla', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/x-www-form-urlencoded',
        },
        body: `yemekId=${yemekId}&isLike=${isLike}`
    })
    .then(response => response.json())
    .then(data => {
        if (data.success) {
            // Sayıları güncelle
            votingSection.querySelector('.like-count').innerText = data.likes;
            votingSection.querySelector('.dislike-count').innerText = data.dislikes;
            
            // Aktif butonu işaretle
            element.classList.add('active');
            
            // Bildirim (Opsiyonel: Daha şık bir şey eklenebilir)
            console.log("Oy kaydedildi.");
        } else {
            alert(data.message);
        }
    })
    .catch(error => {
        console.error('Hata:', error);
        alert("Bir hata oluştu. Lütfen tekrar deneyin.");
    })
    .finally(() => {
        // Butonları tekrar aktif et (eğer hata varsa veya tekrar denetmek isterseniz)
        // Ancak iş kuralı gereği bugünlük 1 oy sınırı var, bu yüzden sayfayı yenileyene kadar 
        // veya başarılı işlem sonrası bu butonları kilitli tutmak daha mantıklı olabilir.
    });
}
