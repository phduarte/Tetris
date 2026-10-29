// Rolagem de um elemento pelas setas do aparelho (o Blazor não rola elementos sozinho).

/** Rola o elemento verticalmente, em pixels (negativo sobe). */
export function scrollBy(element, top) {
    element.scrollBy({ top, behavior: "smooth" });
}
